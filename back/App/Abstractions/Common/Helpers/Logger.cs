using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;

namespace Transport.Api.Abstractions.Common.Helpers;

public class CallerEnricher : ILogEventEnricher
{
	public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
	{
		var skip = 3;
		while (true)
		{
			var stack = new StackFrame(skip);
			if (!stack.HasMethod())
			{
				logEvent.AddPropertyIfAbsent(new LogEventProperty("Caller", new ScalarValue("<unknown method>")));
				return;
			}

			var method = stack.GetMethod();
			if (method!.DeclaringType!.Assembly != typeof(Serilog.Log).Assembly)
			{
				var caller = NeedLogging(method) ? $" {method.DeclaringType.Name}.{method.Name}" : "";
				logEvent.AddPropertyIfAbsent(new LogEventProperty("Caller", new ScalarValue(caller)));
				return;
			}

			skip++;
		}
	}

	private bool NeedLogging(MemberInfo method)
	{
		return method.Module.FullyQualifiedName.Contains("Cicd.Hub");
	}
}

public static class LoggerCallerEnrichmentConfiguration
{
	public static LoggerConfiguration WithCaller(this LoggerEnrichmentConfiguration enrichmentConfiguration)
	{
		return enrichmentConfiguration.With<CallerEnricher>();
	}
}


public static class Log
{
	private static readonly JsonSerializerOptions options = new()
	{
		Converters =
		{
			new JsonStringEnumConverter()
		}
	};

	public static string Format(object value, [CallerArgumentExpression("value")] string name = "")
	{
		return $"{name}={JsonSerializer.Serialize(value, options)}";
	}


	public static void Enter<T>(this ILogger<T> logger, string arguments = "", LogLevel level = LogLevel.Debug, [CallerMemberName] string method = "")
	{
		logger.Log(level, $"Entering - {method}{(arguments.Any() ? ": " : "")}{arguments}");
	}


	public static void Exit<T>(this ILogger<T> logger, string arguments = "", LogLevel level = LogLevel.Debug, [CallerMemberName] string method = "")
	{
		logger.Log(level, $"Exiting - {method}{(arguments.Any() ? ": " : "")}{arguments}");
	}
}