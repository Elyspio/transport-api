using Microsoft.Extensions.Logging;
using Spectre.Console;
using System.Collections.Concurrent;
using System.Globalization;
using Transport.Api.Abstractions.Enums;
using Transport.Api.Abstractions.Interfaces.Repositories;
using Transport.Api.Abstractions.Interfaces.Repositories.Location;
using Transport.Api.Abstractions.Interfaces.Services;
using Transport.Api.Abstractions.Models;
using Transport.Api.Abstractions.Transports;
using Transport.Api.Core.Assemblers;

namespace Transport.Api.Core.Services;

public class StatsService : IStatsService
{
	private readonly ICacheService cacheService;
	private readonly ICityRepository cityRepository;
	private readonly IDepartementRepository departementRepository;
	private readonly IFuelStationRepository fuelStationRepository;
	private readonly ILogger<StatsService> logger;
	private readonly IPriceRepository priceRepository;
	private readonly IRegionRepository regionRepository;
	private readonly IStatisticRepository statisticRepository;

	private readonly StatsAssembler statsAssembler = new();

	public StatsService(ILogger<StatsService> logger, IFuelStationRepository fuelStationRepository, IRegionRepository regionRepository,
		IPriceRepository priceRepository,
		IStatisticRepository statisticRepository, ICacheService cacheService, IDepartementRepository departementRepository, ICityRepository cityRepository)
	{
		this.fuelStationRepository = fuelStationRepository;
		this.regionRepository = regionRepository;
		this.priceRepository = priceRepository;
		this.statisticRepository = statisticRepository;
		this.cacheService = cacheService;
		this.departementRepository = departementRepository;
		this.cityRepository = cityRepository;
		this.logger = logger;
	}

	public async Task RefreshStats()
	{
		await Task.Delay(10);
		throw new NotImplementedException();
	}


	public async Task RefreshWeeklyStats(int year, ProgressTask? ctx)
	{
		await statisticRepository.ClearWeekly(year);

		var locker = new object();
		var progress = 0;

		await Parallel.ForEachAsync(Enumerable.Range(1, 52), async (week, _) =>
		{
			await RefreshWeeklyStatsInternal(year, week);
			lock (locker)
			{
				progress += 1;
				if (ctx != default) ctx.Value = progress;
			}
		});
	}


	public async Task RefreshDailyStats(bool clear)
	{
		if (clear) await statisticRepository.ClearDaily();

		var now = DateTime.Now;
		var lastMonth = now.AddMonths(-1);


		await Parallel.ForEachAsync(Enumerable.Range(0, (now - lastMonth).Days - 1), async (day, _) =>
			{
				var startDate = lastMonth.AddDays(day);
				var endDate = startDate.AddDays(1);

				//logger.LogInformation($"Calculating statistics for day {startDate.ToShortDateString()}");

				var infos = await CalculateBetweenDates(startDate, endDate);

				await statisticRepository.Add(infos, startDate, endDate, StatisticTimeType.Day);

				//logger.LogInformation($"Calculated statistics for day {startDate.ToShortDateString()}");
			}
		);
	}

	public async Task<List<Statistic>> GetWeeklyStats(StatsTimeType statsTimeType)
	{
		if (cacheService.TryGet<List<Statistic>>(statsTimeType.ToString(), out var stats)) return stats!;

		var raw = await statisticRepository.GetByType(statsTimeType);

		stats = statsAssembler.Convert(raw).ToList();

		cacheService.Set(statsTimeType.ToString(), stats);

		return stats;
	}

	public async Task<List<Statistic>> GetDailyStats(StatsTimeType statsTimeType)
	{
		await Task.Delay(1);
		throw new NotImplementedException();
	}

	private async Task RefreshWeeklyStatsInternal(int year, int week)
	{
		// logger.LogInformation($"Calculating statistics for the week {week}/{year}");
		var startDate = ISOWeek.ToDateTime(year, week, DayOfWeek.Monday);

		var endDate = startDate.AddDays(7);

		var infos = await CalculateBetweenDates(startDate, endDate);

		await statisticRepository.Add(infos, startDate, endDate, StatisticTimeType.Week);

		// logger.LogInformation("Calculated statistics for the week {Week}/{Year}", week, year);
	}

	/// <summary>
	///     Calculate statistics for given dates
	/// </summary>
	/// <returns></returns>
	private async Task<StatisticInfo> CalculateBetweenDates(DateTime start, DateTime end)
	{
		//logger.LogInformation("Calculating statistics between {Start} and {End}", start.ToShortDateString(), end.ToShortDateString());

		var infos = new StatisticInfo();

		var prices = await priceRepository.GetBetweenDates(start, end);

		var stations = await fuelStationRepository.GetById(prices.Select(p => p.IdStation).Distinct());

		#region Cities

		var postalCodes = await cityRepository.GetAllPostalCodes();
		var postalCodeDict = new ConcurrentDictionary<string, StatisticData>();
		await Parallel.ForEachAsync(postalCodes, async (postalCode, _) => { postalCodeDict[postalCode] = await GetStatisticsByPostalCode(stations, prices, postalCode); });
		infos.Cities = new Dictionary<string, StatisticData>(postalCodeDict.ToList());

		#endregion Cities

		#region Regions

		var regionDict = new ConcurrentDictionary<RegionId, StatisticData>();
		var regionsIds = (await regionRepository.GetAll()).Select(region => region.Id);
		await Parallel.ForEachAsync(regionsIds, async (region, _) => { regionDict[region] = await GetStatisticByRegion(stations, prices, region); });
		infos.Regions = new Dictionary<RegionId, StatisticData>(regionDict.ToList());

		#endregion

		#region Departements

		var departementDict = new ConcurrentDictionary<string, StatisticData>();
		var departements = await departementRepository.GetAll();
		await Parallel.ForEachAsync(departements, async (departement, _) => { departementDict[departement.Code] = await GetStatisticByDepartement(stations, prices, departement.Code); });
		infos.Departements = new Dictionary<string, StatisticData>(departementDict.ToList());

		#endregion

		return infos;
	}


	private StatisticData CalculateStatistics(IEnumerable<PriceEntity> prices)
	{
		var sortedPrices = prices.OrderBy(p => p.Value).ToList();

		var data = new StatisticData
		{
			Average = new Dictionary<Fuel, double>(),
			Deciles = new Dictionary<Fuel, double>[10],
			Min = new Dictionary<Fuel, double>(),
			Max = new Dictionary<Fuel, double>()
		};


		for (var i = 0; i < 10; i++) data.Deciles[i] = new Dictionary<Fuel, double>();

		foreach (var fuel in Enum.GetValues<Fuel>())
		{
			var pricesByFuel = sortedPrices.Where(p => p.Fuel == fuel).ToList();

			if (!pricesByFuel.Any()) continue;

			data.Average[fuel] = pricesByFuel.Average(p => p.Value);

			data.Min[fuel] = data.Deciles[0][fuel] = pricesByFuel.First().Value;
			data.Max[fuel] = data.Deciles[9][fuel] = pricesByFuel.Last().Value;


			for (var i = 1; i < 9; i++)
			{
				var index = (int) Math.Floor(i * pricesByFuel.Count / 10.0);
				//Console.WriteLine($"index {index} for fuel {fuel} count={pricesByFuel.Count}");
				data.Deciles[i][fuel] = pricesByFuel[index].Value;
			}
		}

		return data;
	}


	private async Task<StatisticData> GetStatisticByRegion(IEnumerable<FuelStationEntity> allStations, IEnumerable<PriceEntity> allPrices, RegionId regionId)
	{
		var departements = await departementRepository.GetByRegion(regionId);

		var stations = allStations.Where(s => departements.Any(departement => s.Location.PostalCode.StartsWith(departement.Code))).ToList();

		var prices = allPrices.Where(p => stations.Select(s => s.Id).Contains(p.IdStation)).ToList();


		return CalculateStatistics(prices);
	}

	private Task<StatisticData> GetStatisticByDepartement(IEnumerable<FuelStationEntity> allStations, IEnumerable<PriceEntity> allPrices, string departement)
	{
		var stations = allStations.Where(s => s.Location.PostalCode.StartsWith(departement)).ToList();

		var prices = allPrices.Where(p => stations.Select(s => s.Id).Contains(p.IdStation)).ToList();

		return Task.Run(() => CalculateStatistics(prices));
	}


	private Task<StatisticData> GetStatisticsByPostalCode(IEnumerable<FuelStationEntity> allStations, IEnumerable<PriceEntity> allPrices, string postalCode)
	{
		var currentStations = allStations.Where(s => s.Location.PostalCode == postalCode).ToList();
		var prices = allPrices.Where(p => currentStations.Select(s => s.Id).Contains(p.IdStation)).ToList();

		return Task.Run(() => CalculateStatistics(prices));
	}
}