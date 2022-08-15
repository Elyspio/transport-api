using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Transport.Api.Abstractions.Enums;
using Transport.Api.Abstractions.Interfaces.Repositories;
using Transport.Api.Abstractions.Models;
using Transport.Api.Db.Repositories.Internal;

namespace Transport.Api.Db.Repositories;

internal class StatisticRepository : BaseRepository<StatisticEntity>, IStatisticRepository
{
	public StatisticRepository(IConfiguration configuration, ILogger<BaseRepository<StatisticEntity>> logger) : base(configuration, logger)
	{
	}


	public async Task<StatisticEntity> Add(StatisticInfo data, DateTime startDate, DateTime endDate, StatisticTimeType timeType)
	{
		var entity = new StatisticEntity
		{
			Time = new TimeMetadata
			{
				Type = timeType,
				Created = DateTime.Now,
				Start = startDate,
				End = endDate
			},
			Statistic = data
		};
		await EntityCollection.InsertOneAsync(entity);

		return entity;
	}

	public async Task<List<StatisticEntity>> GetByType(StatsTimeType type)
	{
		var startDate = type switch
		{
			StatsTimeType.Week => DateTime.Now.AddDays(-7),
			StatsTimeType.Month => DateTime.Now.AddMonths(-1),
			StatsTimeType.Month3 => DateTime.Now.AddMonths(-3),
			StatsTimeType.Month6 => DateTime.Now.AddMonths(-6),
			StatsTimeType.Year => DateTime.Now.AddYears(-1),
			StatsTimeType.Year2 => DateTime.Now.AddYears(-2),
			StatsTimeType.Year5 => DateTime.Now.AddYears(-5),
			StatsTimeType.Year10 => DateTime.Now.AddYears(-10),
			StatsTimeType.AllTime => DateTime.MinValue,
			_ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
		};


		var targetType = new List<StatsTimeType> {StatsTimeType.Month, StatsTimeType.Week}.Contains(type) ? StatisticTimeType.Day : StatisticTimeType.Week;


		return await EntityCollection.AsQueryable().Where(stat => stat.Time.Type == targetType && stat.Time.Start >= startDate && stat.Time.End <= DateTime.Now).ToListAsync();
	}

	public async Task ClearWeekly(int? year)
	{
		var filter = Builders<StatisticEntity>.Filter.Eq(stat => stat.Time.Type, StatisticTimeType.Week);
		if (year != default)
		{
			filter &= Builders<StatisticEntity>.Filter.Gt(e => e.Time.Start, new DateTime(year.Value, 1, 1));
			filter &= Builders<StatisticEntity>.Filter.Lt(e => e.Time.End, new DateTime(year.Value, 12, 30));
		}

		await EntityCollection.DeleteManyAsync(filter);
	}

	public async Task ClearDaily()
	{
		var filter = Builders<StatisticEntity>.Filter.Eq(stat => stat.Time.Type, StatisticTimeType.Day);
		await EntityCollection.DeleteManyAsync(filter);
	}
}