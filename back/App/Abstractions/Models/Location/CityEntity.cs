using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Transport.Api.Abstractions.Models.Location;

public class CityEntity
{
	[Required]
	[BsonId]
	public ObjectId Id;

	[Required] public ObjectId DepartementId { get; set; } = default!;


	[Required] public string Name { get; set; } = default!;

	[Required] public string PostalCode { get; set; } = default!;
}