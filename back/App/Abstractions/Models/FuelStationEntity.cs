using MongoDB.Bson.Serialization.Attributes;
using Transport.Api.Abstractions.Enums;
using Transport.Api.Abstractions.Transports;

namespace Transport.Api.Abstractions.Models;

public class FuelStationEntity
{
    [BsonId] public long Id { get; set; }

    public Location Location { get; set; }

    public List<FuelStationServiceType> Services { get; set; }
}