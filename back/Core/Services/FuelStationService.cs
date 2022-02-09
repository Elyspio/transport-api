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

        public async Task<List<FuelStationData>> GetFuelStations(double lat, double lon, long radius)
        {
            var stations = await client.GetFuelStations();

            return stations.FindAll(station =>
            {
                var distance = Calculate(lat, lon, station.Location.Latitude, station.Location.Longitude);
                return distance < radius * 1000;
            });

        }

        private double Calculate(double lat1, double lon1, double lat2, double lon2)
        {
            var R = 6378.137; // Radius of earth in KM
            var dLat = lat2 * Math.PI / 180 - lat1 * Math.PI / 180;
            var dLon = lon2 * Math.PI / 180 - lon1 * Math.PI / 180;
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
            Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) *
            Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = R * c;
            return d * 1000; // meters
        }


    }
}
