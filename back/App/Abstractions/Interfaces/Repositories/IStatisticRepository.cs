using Transport.Api.Abstractions.Enums;
using Transport.Api.Abstractions.Models;

namespace Transport.Api.Abstractions.Interfaces.Repositories;

public interface IStatisticRepository
{
    Task<StatisticEntity> Add(StatisticInfo data, DateTime startDate, DateTime endDate, StatisticTimeType type);

    Task<List<StatisticEntity>> GetByType(StatsTimeType type);

    Task ClearWeekly(int? year = 0);
    Task ClearDaily();
}