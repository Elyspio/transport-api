import { City, Departement, Prices, Region, Statistic, StatsTimeType } from "../../../core/apis/backend/generated";

export type FuelTypes = keyof Prices;
export type DataType = Record<FuelTypes, number> & {
	date: string;
};

export interface StatisticsTheme {
	selected: {
		region: Region["id"] | "all";
		fuels: FuelTypes[];
		departement: Departement["id"] | "all";
		city: City["id"] | "all";
		timeInterval: StatsTimeType;
		switches: Record<SelectedSwitches, boolean>;
	};
	raw: Statistic[];
	data: DataType[];
	locations: Region[];
	departements: Departement[];
	cities: City[];
}

export type SelectedSwitches = "yAxisFrom0";
