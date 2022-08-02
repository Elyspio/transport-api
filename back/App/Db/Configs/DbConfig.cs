namespace Transport.Api.Db.Configs;

public class DbConfig
{
	public const string Section = "Database";
	public string ConnectionString { get; init; }
	public string DatabaseName { get; init; }
}