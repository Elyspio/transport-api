﻿using Transport.Api.Abstractions.Enums;
using Transport.Api.Abstractions.Models;
using Transport.Api.Abstractions.Transports.FuelStation;

namespace Transport.Api.Abstractions.Interfaces.Repositories;

public interface IPriceRepository
{
	Task<List<PriceEntity>> Add(long idStation, Fuel fuel, IEnumerable<DateTime> dates, List<double> values);
	Task<List<PriceEntity>> Add(IEnumerable<FuelStationData> stations);
	Task Clear();
	Task<long> Clear(int year);
	Task<List<PriceEntity>> GetBetweenDates(DateTime minDate, DateTime maxDate);
	Task<PriceEntity> GetById(string id);

	Task<List<PriceEntity>> GetByYear(int year);
	Task<List<PriceEntity>> GetPricesByYearForStations(int year, IEnumerable<long> stationsIds);
}