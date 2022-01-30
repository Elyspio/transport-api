using Abstraction.Assemblers;
using Abstraction.Enums;
using Abstraction.Models;
using System.Globalization;

namespace Adapters.FuelStation
{
    internal class FuelStationAssembler : BaseAssembler<FuelStations, List<FuelStationData>>
    {
        public override List<FuelStationData> Convert(FuelStations obj)
        {
            var list = new List<FuelStationData>();

            return obj.PdvListe.Pdv.Select(pdv =>
            {
                return new FuelStationData
                {
                    Id = pdv.Id,
                    Location = new Location
                    {
                        Address = pdv.Adresse,
                        Latitude = (double)decimal.Parse(pdv.Latitude, CultureInfo.InvariantCulture) / 100000.0,
                        City = pdv.Ville,
                        Longitude = (double)decimal.Parse(pdv.Longitude, CultureInfo.InvariantCulture) / 100000.0,
                        PostalCode = pdv.Cp,
                    },
                    Services = GetServices(pdv),
                    Prices = GetPrices(pdv),
                };
            }).ToList();

        }

        private Prices GetPrices(Pdv pdv)
        {
            var prices = new Prices();
            if (pdv.Prix.HasValue)
            {
                pdv.Prix.Value.PrixElementArray?.ForEach(prix =>
                {
                    var val = (double)decimal.Parse(prix.Valeur, CultureInfo.InvariantCulture);
                    switch (prix.Nom)
                    {
                        case PrixNom.E10:
                            prices.E10 = val;
                            break;
                        case PrixNom.E85:
                            prices.E85 = val;
                            break;
                        case PrixNom.Gazole:
                            prices.Gazole = val;
                            break;
                        case PrixNom.GpLc:
                            prices.GpLc = val;
                            break;
                        case PrixNom.Sp95:
                            prices.Sp95 = val;
                            break;
                        case PrixNom.Sp98:
                            prices.Sp98 = val;
                            break;
                    }
                });
            }

            return prices;
        }


        private List<FuelStationServiceType> GetServices(Pdv pdv)
        {
            return pdv.Services?.Service.StringArray
                ?.Select(service => (FuelStationServiceType)service)
                .ToList() ?? new List<FuelStationServiceType>();

        }

        public override FuelStations Convert(List<FuelStationData> obj)
        {
            throw new NotImplementedException();
        }
    }
}
