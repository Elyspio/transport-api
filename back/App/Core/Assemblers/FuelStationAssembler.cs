using Transport.Api.Abstractions.Common.Assemblers;
using Transport.Api.Abstractions.Models;
using Transport.Api.Abstractions.Transports.FuelStation;

namespace Transport.Api.Core.Assemblers;

public class FuelStationAssembler : BaseAssembler<FuelStationData, FuelStationEntity>
{
	public override FuelStationData Convert(FuelStationEntity obj)
	{
		return new FuelStationData
		{
			Services = obj.Services,
			Id = obj.Id,
			Location = obj.Location,
			Prices = new Prices()
		};
	}

	public override FuelStationEntity Convert(FuelStationData obj)
	{
		throw new NotImplementedException();
	}
}