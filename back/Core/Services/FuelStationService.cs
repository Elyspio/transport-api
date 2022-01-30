using Abstraction.Interfaces.Services;
using Abstraction.Models;
using Adapters.FuelStation;

namespace Core.Services
{
    public class FuelStationService : IFuelStationService
    {
        private readonly FuelStationClient client;

        public FuelStationService(FuelStationClient client)
        {
            this.client = client;
        }

        public async Task<List<FuelStationData>> GetFuelStations()
        {
            return await client.GetFuelStations();
        }
    }
}
