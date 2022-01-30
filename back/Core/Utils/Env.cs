using System.ComponentModel;

namespace Core.Utils;

public class Env
{
    public T Get<T>(string variableName, T falback)
    {
        var env = Environment.GetEnvironmentVariable(variableName);
        if (env != null)
        {
            var converter = TypeDescriptor.GetConverter(typeof(T));
            return (T)converter.ConvertFromString(env)!;
        }

        return falback;
    }
}