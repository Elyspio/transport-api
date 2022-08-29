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
		var deps = JsonConvert.DeserializeObject<List<DepartementData>>(content)!;

		// Manually add "Corse" department because of stupid french's specificity 
		deps.Add(new DepartementData
		{
			Code = "20",
			Name = "Corse",
			CodeRegion = "94"
		});

		return deps;
	}

	/// <summary>
	///     Get the list of French regions
	/// </summary>
	/// <returns></returns>
	public async Task<List<RegionData>> GetRegions()
	{
		var response = await client.GetAsync("https://geo.api.gouv.fr/regions");
		var content = await response.Content.ReadAsStringAsync();
		return JsonConvert.DeserializeObject<List<RegionData>>(content)!;
	}

	public async Task<List<CityData>> GetCities()
	{
		var response = await client.GetAsync("https://geo.api.gouv.fr/communes");
		var content = await response.Content.ReadAsStringAsync();
		var cities = JsonConvert.DeserializeObject<List<CityData>>(content)!;

		return cities.FindAll(city =>
		{
			var cpRaw = city.CodesPostaux.FirstOrDefault();
			if (cpRaw == default) return false;
			var cp = int.Parse(cpRaw);
			return cp < 97500;
		});
	}
}