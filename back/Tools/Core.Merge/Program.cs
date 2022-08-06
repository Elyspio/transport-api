// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using Transport.Api.Abstractions.Enums;
using Transport.Api.Abstractions.Transports.FuelStation;

Console.WriteLine("Hello, World!");

var files = Directory.GetFiles(@"P:\own\mobile\transport-api\back\Core.Merge").Where(f => new Regex("[0-9]{4}.json").Match(f).Success);


var data = new List<FuelStationData>(20000);

FuelStationData item;
FuelStationData[] content;
foreach (var file in files)
{
	var str = File.ReadAllText(file);
	content = JsonConvert.DeserializeObject<FuelStationData[]>(str)!;
	Console.WriteLine("file " + file);
	foreach (var pdv in content)
	{
		item = data.Find(datum => datum.Id == pdv.Id);
		if (item == null)
		{
			item = pdv;
		}
		else
		{
			foreach (Fuel fuel in Enum.GetValues(typeof(Fuel)))
			{
				item.Prices[fuel].AddRange(pdv.Prices[fuel]);
			}
		}
		data.RemoveAll(datum => datum.Id == pdv.Id);
		data.Add(item);
	}
}
Console.WriteLine($"Writing {data.Count} pdv");

using (TextWriter writer = File.CreateText("./merged.json"))
{
	var serializer = new JsonSerializer() { Formatting = Formatting.None };
	serializer.Serialize(writer, data);
}



