using System.ComponentModel.DataAnnotations;
using Transport.Api.Abstractions.Enums;

namespace Transport.Api.Abstractions.Transports.FuelStation;

public class Prices
{
	public Prices()
	{
		E10 = new List<FuelPriceHistory>();
		E85 = new List<FuelPriceHistory>();
		Gazole = new List<FuelPriceHistory>();
		GpLc = new List<FuelPriceHistory>();
		Sp95 = new List<FuelPriceHistory>();
		Sp98 = new List<FuelPriceHistory>();
	}

	[Required] public List<FuelPriceHistory> E10 { get; set; }

	[Required] public List<FuelPriceHistory> E85 { get; set; }

	[Required] public List<FuelPriceHistory> Gazole { get; set; }

	[Required] public List<FuelPriceHistory> GpLc { get; set; }

	[Required] public List<FuelPriceHistory> Sp95 { get; set; }

	[Required] public List<FuelPriceHistory> Sp98 { get; set; }


	public List<FuelPriceHistory> this[Fuel fuel] => fuel switch
	{
		Fuel.Gazole => Gazole,
		Fuel.E10 => E10,
		Fuel.E85 => E85,
		Fuel.GpLc => GpLc,
		Fuel.Sp95 => Sp95,
		Fuel.Sp98 => Sp98,
		_ => throw new IndexOutOfRangeException($"The value {fuel} is not a Fuel")
	};
}