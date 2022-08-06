using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using Transport.Api.Db.Configs;

namespace Transport.Api.Db.Repositories.Internal;

public class MongoContext
{
	public MongoContext(IConfiguration configuration)
	{
		var conf = new DbConfig();
		configuration.GetSection(DbConfig.Section).Bind(conf);


		var client = new MongoClient(conf.ConnectionString);

		Console.WriteLine($"Connecting to Database '{conf.DatabaseName}'");

		MongoDatabase = client.GetDatabase(conf.DatabaseName);

		var pack = new ConventionPack
		{
			new EnumRepresentationConvention(BsonType.String)
		};
		ConventionRegistry.Register("EnumStringConvention", pack, t => true);
		BsonSerializer.RegisterSerializationProvider(new EnumAsStringSerializationProvider());
	}

	/// <summary>
	///     Récupération de la IMongoDatabase
	/// </summary>
	/// <returns></returns>
	public IMongoDatabase MongoDatabase { get; }
}