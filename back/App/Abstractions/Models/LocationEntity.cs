using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using Transport.Api.Abstractions.Enums;

namespace Transport.Api.Abstractions.Models;

public class LocationEntity
{
	[BsonId]
	[BsonRepresentation(BsonType.String)]
	public Region Id { get; set; }

	public string Code { get; set; } = null!;
	public string Label { get; set; } = null!;
	public List<Departement> Departements { get; set; } = new List<Departement>();
}

public class Departement
{
	[Required] public string Name { get; set; } = null!;

	[Required] public string Code { get; set; } = null!;
}