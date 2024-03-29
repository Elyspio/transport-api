﻿using Spectre.Console;
using Transport.Api.Abstractions.Enums;
using Transport.Api.Abstractions.Transports;

namespace Transport.Api.Abstractions.Interfaces.Services;

public interface IStatsService
{
	public Task RefreshStats();

	public Task RefreshWeeklyStats(int year, ProgressTask? task = null);


	public Task RefreshDailyStats(ProgressContext task);
	public Task<List<Statistic>> GetWeeklyStats(StatsTimeType statsTimeType);
	public Task<List<Statistic>> GetDailyStats(StatsTimeType statsTimeType);
}