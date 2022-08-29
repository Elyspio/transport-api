using Transport.Api.Abstractions.Common.Assemblers;
using Transport.Api.Abstractions.Common.Extensions;
using Transport.Api.Abstractions.Models.Location;
using Transport.Api.Abstractions.Transports.Location;

namespace Transport.Api.Core.Assemblers;

public class LocationAssembler : BaseAssembler<Region, LocationEntity>
{
	public override LocationEntity Convert(Region obj)
	{
		return new LocationEntity
		{
			Id = obj.Id,
			Code = obj.Code,
			Label = obj.Label,
			Departements = obj.Departements.Select(dep => new LocationEntity.DepartementEntity
			{
				Id = dep.Id.AsObjectId(),
				Code = dep.Code,
				Name = dep.Name,
				Cities = dep.Cities.Select(city => new LocationEntity.DepartementEntity.CityEntity
				{
					Id = city.Id.AsObjectId(),
					Name = city.Name,
					PostalCode = city.PostalCode
				}).ToList()
			}).ToList()
		};
	}

	public override Region Convert(LocationEntity obj)
	{
		return new Region
		{
			Id = obj.Id,
			Code = obj.Code,
			Label = obj.Label,
			Departements = obj.Departements.Select(dep => new Departement
			{
				Id = dep.Id.AsGuid(),
				Code = dep.Code,
				Name = dep.Name,
				Cities = dep.Cities.Select(city => new City
				{
					Id = city.Id.AsGuid(),
					Name = city.Name,
					PostalCode = city.PostalCode
				}).ToList()
			}).ToList()
		};
	}
}