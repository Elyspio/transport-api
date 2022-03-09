using Abstractions.Assemblers;
using Abstractions.Models;
using Db.Entities;

namespace Db.Assemblers;

public class FuelStationAssembler : BaseAssembler<FuelStationData, FuelStationEntity>
{

    public override FuelStationData Convert(FuelStationEntity obj)
    {
        return new FuelStationData
        {
            Services = obj.Services,
            Prices = obj.Prices,   
            Id = obj.Id,
            Location = obj.Location,
        };

    }

    public override FuelStationEntity Convert(FuelStationData obj)
    {
        throw new NotImplementedException();
    }
}