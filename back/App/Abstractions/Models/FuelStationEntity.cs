using Abstractions.Enums;
using Abstractions.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace Db.Entities;

public class FuelStationEntity
{

    [BsonId]
    public long Id { get; set; }

    public Location Location { get; set; }

    public List<FuelStationServiceType> Services { get; set; }

}