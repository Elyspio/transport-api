using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using Transport.Api.Abstractions.Enums;

namespace Transport.Api.Abstractions.Models;

public class StatisticEntity
{
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public ObjectId Id { get; set; }

	[Required] public TimeMetadata Time { get; set; } = null!;
	[Required] public StatisticInfo Statistic { get; set; } = null!;
}

public class TimeMetadata
{
	[Required] public StatisticTimeType Type { get; set; }

	[Required] public DateTime Created { get; set; }

	[Required] public DateTime Start { get; set; }

	[Required] public DateTime End { get; set; }
}

public enum StatisticTimeType
{
	Week,
	Day
}

public class StatisticInfo
{
	[Required] public Dictionary<string, StatisticData> Cities { get; set; } = new();

	[Required] public Dictionary<string, StatisticData> Departements { get; set; } = new();

	[Required] public Dictionary<RegionId, StatisticData> Regions { get; set; } = new();
}

public class StatisticData
{
	[Required] public Dictionary<Fuel, double> Average { get; set; } = new();

	[Required] public Dictionary<Fuel, double> Max { get; set; } = new();

	[Required] public Dictionary<Fuel, double> Min { get; set; } = new();

	[Required] public Dictionary<Fuel, double>[] Deciles { get; set; } = new Dictionary<Fuel, double>[10];
}