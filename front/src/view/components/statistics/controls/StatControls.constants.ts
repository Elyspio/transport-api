import { FuelTypes } from "../../../../store/module/statistics/statistics.types";
import { StatsTimeType } from "../../../../core/apis/backend/generated";

export const fuelsLabels: Record<FuelTypes, string> = {
	gpLc: "GPLc",
	sp95: "SP95",
	e85: "E85",
	sp98: "SP98",
	e10: "E10",
	gazole: "Gazole",
};

export const timeLabels: Record<StatsTimeType, string> = {
	AllTime: "Toujours",
	Month: "1 mois",
	Month3: "3 mois",
	Month6: "6 mois",
	Year: "1 an",
	Week: "1 semaine",
	Year2: "2 ans",
	Year5: "5 ans",
	Year10: "10 ans",
};

export const timeOrdered = [
	StatsTimeType.Week,
	StatsTimeType.Month,
	StatsTimeType.Month3,
	StatsTimeType.Month6,
	StatsTimeType.Year,
	StatsTimeType.Year2,
	StatsTimeType.Year5,
	StatsTimeType.Year10,
	StatsTimeType.AllTime,
];
