using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using Transport.Api.Abstractions.Enums;

namespace Transport.Api.Abstractions.Transports.FuelStation;

public class Prices
{
	[Required] public ConcurrentBag<FuelPriceHistory> E10 { get; set; } = new();

	[Required] public ConcurrentBag<FuelPriceHistory> E85 { get; set; } = new();

	[Required] public ConcurrentBag<FuelPriceHistory> Gazole { get; set; } = new();

	[Required] public ConcurrentBag<FuelPriceHistory> GpLc { get; set; } = new();

	[Required] public ConcurrentBag<FuelPriceHistory> Sp95 { get; set; } = new();

	[Required] public ConcurrentBag<FuelPriceHistory> Sp98 { get; set; } = new();


	public ConcurrentBag<FuelPriceHistory> this[Fuel fuel] => fuel switch
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