using Transport.Api.Abstractions.Enums;
using Transport.Api.Abstractions.Transports;

namespace Transport.Api.Abstractions.Interfaces.Services;

public interface IStatsService
{
    public Task RefreshStats();

    public Task RefreshWeeklyStats(bool clear = false, int? year = null);


    public Task RefreshDailyStats(bool clear = false);
    public Task<List<Statistic>> GetWeeklyStats(StatsTimeType statsTimeType);
    public Task<List<Statistic>> GetDailyStats(StatsTimeType statsTimeType);
}