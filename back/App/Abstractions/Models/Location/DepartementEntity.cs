using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using Transport.Api.Abstractions.Enums;

namespace Transport.Api.Abstractions.Models.Location;

public class DepartementEntity
{
	[Required]
	[BsonId]
	public ObjectId Id;

	[Required] public RegionId RegionId { get; set; } = default;

	[Required] public string Name { get; set; } = default!;
	[Required] public string Code { get; set; } = default!;
}