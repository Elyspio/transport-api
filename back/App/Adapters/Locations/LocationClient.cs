using Newtonsoft.Json;

namespace Transport.Api.Adapters.Locations;

public class LocationClient
{
	private readonly HttpClient client;

	public LocationClient(HttpClient client)
	{
		this.client = client;
	}


	public async Task<List<DepartementData>> GetDepartements()
	{
		var response = await client.GetAsync("https://geo.api.gouv.fr/departements");
		var content = await response.Content.ReadAsStringAsync();
		return JsonConvert.DeserializeObject<List<DepartementData>>(content);
	}


	/// <summary>
	///     Get the list of French regions
	/// </summary>
	/// <returns></returns>
	public async Task<List<RegionData>> GetRegions()
	{
		var response = await client.GetAsync("https://geo.api.gouv.fr/regions");
		var content = await response.Content.ReadAsStringAsync();
		return JsonConvert.DeserializeObject<List<RegionData>>(content);
	}
}