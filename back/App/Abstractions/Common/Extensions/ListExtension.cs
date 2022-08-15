namespace Transport.Api.Abstractions.Common.Extensions;

public static class ListExtension
{
	public static void ForEach<T>(this IEnumerable<T> ie, Action<T, int> action)
	{
		var i = 0;
		foreach (var e in ie) action(e, i++);
	}
}