using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Transport.Api.Db.Repositories.Internal;

public abstract class BaseRepository<T>
{
	protected readonly string CollectionName;
	protected readonly MongoContext context;
	private readonly ILogger<BaseRepository<T>> logger;

	protected BaseRepository(IConfiguration configuration, ILogger<BaseRepository<T>> logger, string name = null)
	{
		context = new MongoContext(configuration);
		CollectionName = name ?? typeof(T).Name[..^"Entity".Length];
		this.logger = logger;
		var pack = new ConventionPack
		{
			new EnumRepresentationConvention(BsonType.String)
		};

		ConventionRegistry.Register("EnumStringConvention", pack, t => true);
		BsonSerializer.RegisterSerializationProvider(new EnumAsStringSerializationProvider());
	}

	protected IMongoCollection<T> EntityCollection => context.MongoDatabase.GetCollection<T>(CollectionName);


	protected void CreateIndexIfMissing(ICollection<string> properties, bool unique = false)
	{
		var indexName = string.Join("-", properties);
		var indexes = EntityCollection.Indexes.List().ToList();
		var foundIndex = indexes.Any(index =>
		{
			var element = index["key"].AsBsonDocument;
			if(!element.TryGetValue("name", out BsonValue val))
			{
				return false;
			}
			else return val.AsString.Contains(indexName);
		});

		var indexBuilder = Builders<T>.IndexKeys;

		var newIndex = indexBuilder.Combine(properties.Select(property => indexBuilder.Ascending(property)));


		var options = new CreateIndexOptions
		{
			Unique = unique,
			Name = indexName
		};

		var indexModel = new CreateIndexModel<T>(newIndex, options);


		if (!foundIndex)
		{
			logger.LogWarning($"Property {CollectionName}.{indexName} is not indexed, creating one");
			EntityCollection.Indexes.CreateOne(indexModel);
			logger.LogWarning($"Property {CollectionName}.{indexName} is now indexed");
		}
	}
}

public class EnumAsStringSerializationProvider : BsonSerializationProviderBase
{
	public override IBsonSerializer GetSerializer(Type type, IBsonSerializerRegistry serializerRegistry)
	{
		if (!type.IsEnum) return null;

		var enumSerializerType = typeof(EnumSerializer<>).MakeGenericType(type);
		var enumSerializerConstructor = enumSerializerType.GetConstructor(new[] { typeof(BsonType) });
		var enumSerializer = (IBsonSerializer) enumSerializerConstructor.Invoke(new object[] { BsonType.String });

		return enumSerializer;
	}
}