using System.Globalization;
using Transport.Api.Abstractions.Assemblers;
using Transport.Api.Abstractions.Enums;
using Transport.Api.Abstractions.Transports.FuelStation;

namespace Transport.Api.Adapters.FuelStation;

internal class FuelStationAssembler : BaseAssembler<FuelStations, List<FuelStationData>>
{
    public override List<FuelStationData> Convert(FuelStations obj)
    {
        var list = new List<FuelStationData>();

        var pdvs = obj.PdvListe.Pdv.Select(pdv =>
                {
                    try
                    {
                        return new FuelStationData
                        {
                            Id = pdv.Id,
                            Location = new Location
                            {
                                Address = pdv.Adresse,
                                Latitude = (double) decimal.Parse(pdv.Latitude, CultureInfo.InvariantCulture) / 100000.0,
                                City = pdv.Ville,
                                Longitude = (double) decimal.Parse(pdv.Longitude, CultureInfo.InvariantCulture) / 100000.0,
                                PostalCode = pdv.Cp
                            },
                            Services = GetServices(pdv),
                            Prices = GetPrices(pdv)
                        };
                    }
                    catch (Exception e)
                    {
                        //Console.Error.WriteLine(e);
                        return null;
                    }
                }
            )
            .Where(pdv => pdv != null);
        return pdvs.ToList();
    }

    private Prices GetPrices(Pdv pdv)
    {
        var prices = new Prices();
        if (pdv.Prix.HasValue)
            pdv.Prix.Value.PrixElementArray?.ForEach(prix =>
                {
                    var val = (double) decimal.Parse(prix.Valeur, CultureInfo.InvariantCulture);
                    var date = prix.Maj.DateTime;
                    switch (prix.Nom)
                    {
                        case FuelType.E10:
                            prices.E10.Add(new FuelPriceHistory
                                {
                                    Date = date,
                                    Value = val
                                }
                            );
                            break;
                        case FuelType.E85:
                            prices.E85.Add(new FuelPriceHistory
                                {
                                    Date = date,
                                    Value = val
                                }
                            );
                            break;
                        case FuelType.Gazole:
                            prices.Gazole.Add(new FuelPriceHistory
                                {
                                    Date = date,
                                    Value = val
                                }
                            );
                            break;
                        case FuelType.GpLc:
                            prices.GpLc.Add(new FuelPriceHistory
                                {
                                    Date = date,
                                    Value = val
                                }
                            );
                            break;
                        case FuelType.Sp95:
                            prices.Sp95.Add(new FuelPriceHistory
                                {
                                    Date = date,
                                    Value = val
                                }
                            );
                            break;
                        case FuelType.Sp98:
                            prices.Sp98.Add(new FuelPriceHistory
                                {
                                    Date = date,
                                    Value = val
                                }
                            );
                            break;
                    }
                }
            );

        return prices;
    }


    private List<FuelStationServiceType> GetServices(Pdv pdv)
    {
        return pdv.Services?.Service.StringArray?.Select(service => (FuelStationServiceType) service).ToList() ?? new List<FuelStationServiceType>();
    }

    public override FuelStations Convert(List<FuelStationData> obj)
    {
        throw new NotImplementedException();
    }
}