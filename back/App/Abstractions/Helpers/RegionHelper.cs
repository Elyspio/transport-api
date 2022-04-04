using Transport.Api.Abstractions.Interfaces.Repositories;

namespace Transport.Api.Abstractions.Helpers;

public static class RegionHelper
{
    private static readonly Dictionary<Region, List<string>> departementsMap = new()
    {
        {
            Region.AuvergneRhoneAlpes, new List<string>
            {
                "01",
                "03",
                "07",
                "15",
                "26",
                "38",
                "42",
                "43",
                "63",
                "69",
                "73",
                "74"
            }
        },
        {
            Region.BourgogneFrancheComte, new List<string>
            {
                "21",
                "25",
                "39",
                "58",
                "70",
                "71",
                "89",
                "90"
            }
        },
        {
            Region.Bretagne, new List<string>
            {
                "22",
                "29",
                "35",
                "56"
            }
        },
        {
            Region.CentreValDeLoire, new List<string>
            {
                "18",
                "28",
                "36",
                "37",
                "41",
                "45"
            }
        },
        {
            Region.Corse, new List<string>
            {
                "2A",
                "2B"
            }
        },
        {
            Region.GrandEst, new List<string>
            {
                "08",
                "10",
                "51",
                "52",
                "54",
                "55",
                "57",
                "67",
                "68",
                "88"
            }
        },
        {
            Region.HautDeFrance, new List<string>
            {
                "02",
                "59",
                "60",
                "62",
                "80"
            }
        },
        {
            Region.IleDeFrance, new List<string>
            {
                "75",
                "77",
                "78",
                "91",
                "92",
                "93",
                "94",
                "95"
            }
        },
        {
            Region.Normandie, new List<string>
            {
                "14",
                "27",
                "50",
                "61",
                "76"
            }
        },
        {
            Region.NouvelleAquitaine, new List<string>
            {
                "16",
                "17",
                "19",
                "23",
                "24",
                "33",
                "40",
                "47",
                "64",
                "79",
                "86",
                "87"
            }
        },
        {
            Region.Occitanie, new List<string>
            {
                "09",
                "11",
                "12",
                "30",
                "31",
                "32",
                "34",
                "46",
                "48",
                "65",
                "66",
                "81",
                "82"
            }
        },
        {
            Region.PaysDeLaLoire, new List<string>
            {
                "44",
                "49",
                "53",
                "72",
                "85"
            }
        },
        {
            Region.ProvenceAlpesCoteAzur, new List<string>
            {
                "04",
                "05",
                "06",
                "13",
                "83",
                "84"
            }
        }
    };

    public static List<string> GetDepartements(this Region region)
    {
        return departementsMap[region];
    }

    public static List<string> GetAllDepartements()
    {
        return departementsMap.Values.SelectMany(x => x).Distinct().ToList();
    }
}