﻿using System.ComponentModel;

namespace Transport.Api.Abstractions.Helpers;

public class Env
{
    public static T Get<T>(string variableName, T falback)
    {
        var env = Environment.GetEnvironmentVariable(variableName);
        if (env != null)
        {
            var converter = TypeDescriptor.GetConverter(typeof(T));
            return (T) converter.ConvertFromString(env)!;
        }

        return falback;
    }
}