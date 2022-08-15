using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Transport.Api.Abstractions.Interfaces.Injections;
using Transport.Api.Adapters.FuelStation;
using Transport.Api.Adapters.Locations;

namespace Transport.Api.Adapters.Injections;

public class AdapterModule : IDotnetModule
{
	public void Load(IServiceCollection services, IConfiguration configuration)
	{
		services.AddHttpClient<FuelStationClient>();
		services.AddHttpClient<LocationClient>();
	}
}