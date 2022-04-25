namespace Transport.Api.Abstractions.Interfaces.Services;

public interface ICacheService
{
	public bool TryGet<T>(string key, out T? value);
	public T? Get<T>(string key);
	public void Set(string key, object value);
}