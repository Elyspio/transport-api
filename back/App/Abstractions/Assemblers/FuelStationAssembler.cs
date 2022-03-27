using Transport.Api.Abstractions.Models;
using Transport.Api.Abstractions.Transports;

namespace Transport.Api.Abstractions.Assemblers;

public class FuelStationAssembler : BaseAssembler<FuelStationData, FuelStationEntity>
{
    public override FuelStationData Convert(FuelStationEntity obj)
    {
        return new FuelStationData
        {
            Services = obj.Services,
            Id = obj.Id,
            Location = obj.Location
        };
    }

    public override FuelStationEntity Convert(FuelStationData obj)
    {
        throw new NotImplementedException();
    }
}