using System.Runtime.Caching;
using Transport.Api.Abstractions.Interfaces.Services;

namespace Transport.Api.Core.Services;

public class CacheService : ICacheService
{
	private readonly MemoryCache cache = new("CacheService");


	public bool TryGet<T>(string key, out T? value)
	{
		value = Get<T>(key);
		return value != null;
	}

	public T? Get<T>(string key)
	{
		if (key == null) throw new ArgumentNullException(nameof(key));
		return (T) cache.Get(key);
	}

	public void Set(string key, object value)
	{
		if (key == null) throw new ArgumentNullException(nameof(key));

		cache.Set(key, value, new CacheItemPolicy { SlidingExpiration = TimeSpan.FromHours(1) });
	}
}