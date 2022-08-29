using MongoDB.Bson.Serialization.Attributes;
using Transport.Api.Abstractions.Enums;
using Transport.Api.Abstractions.Transports.FuelStation;

namespace Transport.Api.Abstractions.Models;

public class FuelStationEntity
{
	[BsonId] public long Id { get; set; } = default!;

	public FuelStationLocation Location { get; set; } = default!;

	public List<FuelStationServiceType> Services { get; set; } = default!;
}