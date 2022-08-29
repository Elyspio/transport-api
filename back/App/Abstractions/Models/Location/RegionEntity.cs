using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Transport.Api.Abstractions.Enums;

namespace Transport.Api.Abstractions.Models.Location;

public class RegionEntity
{
	[BsonId]
	[BsonRepresentation(BsonType.String)]
	public RegionId Id;


	public string Code { get; set; } = default!;
	public string Label { get; set; } = default!;
}