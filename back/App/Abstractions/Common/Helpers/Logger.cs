using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Spectre.Console;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using ILogger = Microsoft.Extensions.Logging.ILogger;

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


	public static MethodLogger Enter<T>(this ILogger<T> logger, string arguments = "", LogLevel level = LogLevel.Debug, [CallerMemberName] string method = "")
	{
		return new MethodLogger(method, arguments, level, logger);
	}

	public static Progress CreateProgress()
	{
		return AnsiConsole.Progress()
			.AutoRefresh(true) // Turn off auto refresh
			.AutoClear(false) // Do not remove the task list when done
			.HideCompleted(false) // Hide tasks as they are completed
			.Columns(new TaskDescriptionColumn() {Alignment = Justify.Left}, new ProgressBarColumn(), new PercentageColumn(), new ElapsedTimeColumn(), new SpinnerColumn());
	}
}

public class MethodLogger
{
	private readonly string arguments;
	private readonly Guid id;
	private readonly LogLevel level;
	private readonly ILogger logger;
	private readonly string method;

	public MethodLogger(string method, string arguments, LogLevel level, ILogger logger)
	{
		this.method = method;
		this.arguments = arguments;
		this.level = level;
		this.logger = logger;
		id = Guid.NewGuid();


		Enter();
	}


	private void Enter()
	{
		logger.Log(level, $"[{id}] Entering - {method}{(arguments.Any() ? ": " : "")}{arguments}");
	}

	public void Exit()
	{
		logger.Log(level, $"[{id}] Exiting - {method}{(arguments.Any() ? ": " : "")}{arguments}");
	}
}