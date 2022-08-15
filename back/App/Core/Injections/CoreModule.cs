﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Transport.Api.Abstractions.Interfaces.Injections;

namespace Transport.Api.Core.Injections;

public class CoreModule : IDotnetModule
{
	public void Load(IServiceCollection services, IConfiguration configuration)
	{
		var nsp = typeof(CoreModule).Namespace!;
		var baseNamespace = nsp[..nsp.LastIndexOf(".")];
		services.Scan(scan => scan.FromAssemblyOf<CoreModule>().AddClasses(classes => classes.InNamespaces(baseNamespace + ".Services")).AsImplementedInterfaces()
			.WithSingletonLifetime());
	}
}