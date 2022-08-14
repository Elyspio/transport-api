namespace Transport.Api.Abstractions.Common.Helpers
{
	public static class EnumHelper
	{
		public static List<T> GetValues<T>(Type type)
		{
			var enums = Enum.GetValues(type);


			var ret = new List<T>();

			foreach(var elem in enums)
			{
				ret.Add((T) elem);
			}

			return ret;
		}

	}
}
