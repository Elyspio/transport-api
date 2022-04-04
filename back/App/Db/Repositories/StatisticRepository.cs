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
    public StatisticRepository(IConfiguration configuration, ILogger<BaseRepository<StatisticEntity>> logger) : base(configuration, logger) { }


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
            StatsTimeType.Year => DateTime.Now.AddYears(-1),
            StatsTimeType.Month => DateTime.Now.AddMonths(-1),
            StatsTimeType.Month3 => DateTime.Now.AddMonths(-3),
            StatsTimeType.Month6 => DateTime.Now.AddMonths(-6),
            StatsTimeType.Week => DateTime.Now.AddDays(-7),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };

        return await EntityCollection.AsQueryable()
            .Where(stat => stat.Time.Type == StatisticTimeType.Week && stat.Time.Start >= startDate && stat.Time.End <= DateTime.Now)
            .ToListAsync();
    }

    public async Task ClearWeekly()
    {
        var filter = Builders<StatisticEntity>.Filter.Eq(stat => stat.Time.Type, StatisticTimeType.Week);
        await EntityCollection.DeleteManyAsync(filter);
    }

    public async Task ClearDaily()
    {
        var filter = Builders<StatisticEntity>.Filter.Eq(stat => stat.Time.Type, StatisticTimeType.Day);
        await EntityCollection.DeleteManyAsync(filter);
    }
}