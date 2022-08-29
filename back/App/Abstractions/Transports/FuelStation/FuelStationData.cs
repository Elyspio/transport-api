using System.ComponentModel.DataAnnotations;
using Transport.Api.Abstractions.Enums;

namespace Transport.Api.Abstractions.Transports.FuelStation;

public class FuelStationData
{
	public FuelStationData(FuelStationData all)
	{
		Id = all.Id;
		Location = all.Location;
		Prices = all.Prices;
		Services = all.Services;
	}

	public FuelStationData()
	{
	}

	[Required] public long Id { get; set; }

	[Required] public FuelStationLocation Location { get; set; } = null!;


	[Required] public Prices Prices { get; set; } = null!;


	[Required] public List<FuelStationServiceType> Services { get; set; } = null!;
}