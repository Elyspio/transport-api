using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Globalization;
using Transport.Api.Abstractions.Enums;
using Transport.Api.Abstractions.Interfaces.Repositories;
using Transport.Api.Abstractions.Interfaces.Services;
using Transport.Api.Abstractions.Models;
using Transport.Api.Abstractions.Transports;
using Transport.Api.Core.Assemblers;

namespace Transport.Api.Core.Services;

public class StatsService : IStatsService
{
    private readonly IFuelStationRepository fuelStationRepository;
    private readonly ILocationRepository locationRepository;
    private readonly ILogger<StatsService> logger;
    private readonly IPriceRepository priceRepository;
    private readonly IStatisticRepository statisticRepository;
    private readonly StatsAssembler statsAssembler = new();

    public StatsService(ILogger<StatsService> logger, IFuelStationRepository fuelStationRepository, ILocationRepository locationRepository, IPriceRepository priceRepository,
        IStatisticRepository statisticRepository)
    {
        this.fuelStationRepository = fuelStationRepository;
        this.locationRepository = locationRepository;
        this.priceRepository = priceRepository;
        this.statisticRepository = statisticRepository;
        this.logger = logger;
    }

    public async Task RefreshStats()
    {
        await Task.WhenAll(RefreshDailyStats(true), RefreshWeeklyStats(true));
    }


    public async Task RefreshWeeklyStats(bool clear = false, int? year = null)
    {
        if (clear) await statisticRepository.ClearWeekly(year);

        if (year is null)
            for (year = 2007; year <= DateTime.Now.Year; year++)
                await Parallel.ForEachAsync(Enumerable.Range(1, 52), async (week, _) => await RefreshWeeklyStatsInternal(year.Value, week));
        else
            await Parallel.ForEachAsync(Enumerable.Range(1, 52), async (week, _) => await RefreshWeeklyStatsInternal(year.Value, week));
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

            logger.LogInformation($"Calculating statistics for day {startDate.ToShortDateString()}");

            var infos = await CalculateBetweenDates(startDate, endDate);

            await statisticRepository.Add(infos, startDate, endDate, StatisticTimeType.Day);

            logger.LogInformation($"Calculated statistics for day {startDate.ToShortDateString()}");
        }
        );
    }

    public async Task<List<Statistic>> GetWeeklyStats(StatsTimeType statsTimeType)
    {
        var stats = await statisticRepository.GetByType(statsTimeType);

        return statsAssembler.Convert(stats).ToList();
    }

    public async Task<List<Statistic>> GetDailyStats(StatsTimeType statsTimeType)
    {
        throw new NotImplementedException();
    }

    private async Task RefreshWeeklyStatsInternal(int year, int week)
    {
        logger.LogInformation($"Calculating statistics for the week {week}/{year}");
        var startDate = ISOWeek.ToDateTime(year, week, DayOfWeek.Monday);

        var endDate = startDate.AddDays(7);

        var infos = await CalculateBetweenDates(startDate, endDate);

        await statisticRepository.Add(infos, startDate, endDate, StatisticTimeType.Week);

        logger.LogInformation($"Calculated statistics for the week {week}/{year}");
    }

    /// <summary>
    ///     Calculate statistics for given dates
    /// </summary>
    /// <returns></returns>
    private async Task<StatisticInfo> CalculateBetweenDates(DateTime start, DateTime end)
    {
        logger.LogInformation($"Calculating statistics between {start.ToShortDateString()} and {end.ToShortDateString()}");

        var infos = new StatisticInfo();

        var prices = await priceRepository.GetBetweenDates(start, end);

        var stations = await fuelStationRepository.GetById(prices.Select(p => p.IdStation).Distinct());

        # region Cities

        //var postalCodes = await locationRepository.GetPostalCodes();
        //var postalCodeDict = new ConcurrentDictionary<string, StatisticData>();
        //Parallel.ForEach(postalCodes, postalCode => { postalCodeDict[postalCode] = GetStatisticsByPostalCode(stations, prices, postalCode); });
        //infos.Cities = new Dictionary<string, StatisticData>(postalCodeDict.ToList());
        infos.Cities = new Dictionary<string, StatisticData>();

        #endregion Cities

        #region Regions

        var regionDict = new ConcurrentDictionary<Region, StatisticData>();
        var regionsIds = (await locationRepository.GetRegions()).Select(region => region.Id);
        await Parallel.ForEachAsync(regionsIds, async (region, _) => { regionDict[region] = await GetStatisticByRegion(stations, prices, region); });
        infos.Regions = new Dictionary<Region, StatisticData>(regionDict.ToList());

        #endregion

        #region Depatements

        var departementDict = new ConcurrentDictionary<string, StatisticData>();
        var departements = await locationRepository.GetAllDepartements();
        Parallel.ForEach(departements, departement => { departementDict[departement.Code] = GetStatisticByDepartement(stations, prices, departement.Code); });
        infos.Departements = new Dictionary<string, StatisticData>(departementDict.ToList());

        #endregion


        logger.LogInformation($"Calculated statistics between {start.ToShortDateString()} and {end.ToShortDateString()}");


        return infos;
    }


    private StatisticData CalculateStatistics(IEnumerable<PriceEntity> prices)
    {
        var sortedPrices = prices.OrderBy(p => p.Value).ToList();

        var data = new StatisticData
        {
            Average = new(),
            Deciles = new Dictionary<Fuel, double>[10],
            Min = new(),
            Max = new()
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


    private async Task<StatisticData> GetStatisticByRegion(IEnumerable<FuelStationEntity> allStations, IEnumerable<PriceEntity> allPrices, Region region)
    {
        var departements = await locationRepository.GetDepartements(region);

        var stations = allStations.Where(s => departements.Any(departement => s.Location.PostalCode.StartsWith(departement.Code))).ToList();

        var prices = allPrices.Where(p => stations.Select(s => s.Id).Contains(p.IdStation)).ToList();


        return CalculateStatistics(prices);
    }

    private StatisticData GetStatisticByDepartement(IEnumerable<FuelStationEntity> allStations, IEnumerable<PriceEntity> allPrices, string departement)
    {
        var stations = allStations.Where(s => s.Location.PostalCode.StartsWith(departement)).ToList();

        var prices = allPrices.Where(p => stations.Select(s => s.Id).Contains(p.IdStation)).ToList();

        return CalculateStatistics(prices);
    }


    private StatisticData GetStatisticsByPostalCode(IEnumerable<FuelStationEntity> allStations, IEnumerable<PriceEntity> allPrices, string postalCode)
    {
        var currentStations = allStations.Where(s => s.Location.PostalCode == postalCode).ToList();
        var prices = allPrices.Where(p => currentStations.Select(s => s.Id).Contains(p.IdStation)).ToList();

        return CalculateStatistics(prices);
    }
}