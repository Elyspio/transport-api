namespace Db.Cache.Configs;

internal class DbCacheEndpoint
{
	public const string Section = "Endpoints";

	public string FilesApi { get; set; } = default!;

	public string AuthenticationApi { get; set; } = default!;
}