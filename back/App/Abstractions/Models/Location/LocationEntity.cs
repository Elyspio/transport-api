using MongoDB.Bson;
using Transport.Api.Abstractions.Enums;

namespace Transport.Api.Abstractions.Models.Location;

public class LocationEntity
{
	public RegionId Id { get; set; }

	public string Code { get; set; }

	public string Label { get; set; }

	public List<DepartementEntity> Departements { get; set; }

	public class DepartementEntity
	{
		public ObjectId Id { get; set; }

		public string Name { get; set; }

		public string Code { get; set; }

		public List<CityEntity> Cities { get; set; }

		public class CityEntity
		{
			public ObjectId Id { get; set; }

			public string Name { get; set; }

			public string PostalCode { get; set; }
		}
	}
}