using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Transport.Api.Abstractions.Enums;

namespace Transport.Api.Abstractions.Models;

public class PriceEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; }

    public Fuel Fuel { get; set; }

    public long IdStation { get; set; }

    public DateTime Date { get; set; }
    public double Value { get; set; }
}