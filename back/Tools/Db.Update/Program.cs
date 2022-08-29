using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Spectre.Console;
using System.Collections.Immutable;
using Transport.Api.Abstractions.Interfaces.Injections;
using Transport.Api.Abstractions.Interfaces.Services;
using Transport.Api.Adapters.Injections;
using Transport.Api.Core.Injections;
using Transport.Api.Db.Injections;

AnsiConsole.Write(new FigletText("Database Statistics Updater").LeftAligned().Color(Color.Blue3));

var host = Host.CreateDefaultBuilder(args)
	.ConfigureServices((ctx, service) =>
		{
			// Inject Adapters
			service.AddModule<AdapterModule>(ctx.Configuration);
			service.AddModule<CoreModule>(ctx.Configuration);
			service.AddModule<DatabaseModule>(ctx.Configuration);
		}
	)
	.Build();

var scope = host.Services.CreateScope();


var locationService = scope.ServiceProvider.GetRequiredService<IDatabaseUpdateService>();

await locationService.RefreshLocations();

var service = scope.ServiceProvider.GetRequiredService<IStatsService>();

var years = Enumerable.Range(2007, DateTime.Now.Year - 2007 + 1).ToList();
// var years = Enumerable.Range(2007, 2);

var yearsSelected = AnsiConsole.Prompt(
	new MultiSelectionPrompt<string>()
		.PageSize(30)
		.Title("Which [green]year[/] to refresh")
		.AddChoiceGroup("Weekly", years.Select(year => year.ToString()))
);


var yearsToRefresh = yearsSelected.Select(int.Parse).ToList();

yearsToRefresh.Sort();

await AnsiConsole.Progress()
	.AutoRefresh(true) // Turn off auto refresh
	.AutoClear(false) // Do not remove the task list when done
	.HideCompleted(false) // Hide tasks as they are completed
	.Columns(new TaskDescriptionColumn(), new ProgressBarColumn(), new PercentageColumn(), new ElapsedTimeColumn(), new SpinnerColumn())
	.StartAsync(async ctx =>
	{
		await Parallel.ForEachAsync(yearsToRefresh.ToImmutableArray(), new ParallelOptions { MaxDegreeOfParallelism = 4 }, async (year, _) =>
		{
			var task = ctx.AddTask(year.ToString(), new ProgressTaskSettings { MaxValue = 52 });
			await service.RefreshWeeklyStats(year, task);
			task.StopTask();
		});
	});