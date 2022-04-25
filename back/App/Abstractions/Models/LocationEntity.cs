using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Transport.Api.Abstractions.Enums;

namespace Transport.Api.Abstractions.Models;

public class LocationEntity
{
	[BsonId]
	[BsonRepresentation(BsonType.String)]
	public Region Id { get; set; }

	public string Code { get; set; }
	public string Label { get; set; }
	public List<Departement> Departements { get; set; }
}

public class Departement
{
	[Required] public string Name { get; set; }

	[Required] public string Code { get; set; }
}