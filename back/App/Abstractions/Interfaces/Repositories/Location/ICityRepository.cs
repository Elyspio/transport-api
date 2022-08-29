using MongoDB.Bson;
using Transport.Api.Abstractions.Models.Location;

namespace Transport.Api.Abstractions.Interfaces.Repositories.Location;

public interface ICityRepository
{
	Task<List<CityEntity>> Add(IEnumerable<(string name, string postalCode, ObjectId DepartementId)> data);
	Task Clear();
	Task<List<CityEntity>> GetAll();

	Task<List<string>> GetAllPostalCodes();
}