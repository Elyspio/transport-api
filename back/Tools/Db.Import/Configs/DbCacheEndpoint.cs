namespace Transport.Api.Db.Cache.Configs;

internal class DbCacheEndpoint
{
	public const string Section = "Endpoints";

	public string FilesApi { get; set; }

	public string AuthenticationApi { get; set; }
}