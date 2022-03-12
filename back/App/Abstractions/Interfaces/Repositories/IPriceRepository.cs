using Abstractions.Enums;
using Db.Entities;

namespace Abstractions.Interfaces.Repositories
{
    public interface IPriceRepository
    {
        Task<PriceEntity> Add(long idStation, Fuel fuel, DateTime date, double value);
        Task Clear();
        Task<List<PriceEntity>> GetBetweenDates(DateTime minDate, DateTime maxDate);
        Task<PriceEntity> GetById(string id);
    }
}