using Transport.Api.Abstractions.Enums;
using Transport.Api.Abstractions.Models;
using Transport.Api.Abstractions.Transports;

namespace Transport.Api.Abstractions.Interfaces.Repositories;

public interface IPriceRepository
{
    Task<PriceEntity> Add(long idStation, Fuel fuel, DateTime date, double value);
    Task<List<PriceEntity>> Add(IEnumerable<FuelStationData> stations);
    Task Clear();
    Task<long> Clear(int year);
    Task<List<PriceEntity>> GetBetweenDates(DateTime minDate, DateTime maxDate);
    Task<PriceEntity> GetById(string id);
}