using Abstraction.Enums;
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

        public async Task<List<FuelStationDataDistance>> GetFuelStations(double lat, double lon, long radius)
        {
            var stations = await client.GetFuelStations();

            return stations
                .Select(station => new FuelStationDataDistance(
                    station,
                    Calculate(lat, lon, station.Location.Latitude, station.Location.Longitude))
                )
                .Where(station => station.Distance < radius * 1000)
                .ToList();

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


        public async Task<List<FuelStationHistory>> GetHistories()
        {
            var stations = await client.GetFuelStationsAllTime();

            var histories = new List<FuelStationHistory>();

            stations.ForEach(station =>
            {

                var history = histories.Find(h => h.Id == station.Id);

                if (history == null)
                {
                    history = new FuelStationHistory
                    {
                        Id = station.Id,
                        Location = station.Location,
                        Prices = new Dictionary<Fuel, List<FuelPriceHistory>>()
                    };

                    foreach (Fuel type in Enum.GetValues(typeof(Fuel)))
                    {
                        history.Prices.Add(type, new List<FuelPriceHistory>());
                    }

                    histories.Add(history);
                }

                foreach (Fuel type in Enum.GetValues(typeof(Fuel)))
                {
                    var prices = station.Prices[type];
                    history.Prices[type].AddRange(prices);
                }

            });


            return histories;
        }

        public async Task Fetch()
        {
            await client.Fetch();
        }

    }
}
