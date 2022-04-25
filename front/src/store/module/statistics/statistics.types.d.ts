import { Departement, Prices, RegionTransport, Statistic, StatsTimeType } from "../../../core/apis/backend/generated";

export type FuelTypes = keyof Prices;
export type DataType = Record<FuelTypes, number> & {
	date: string;
};

export interface StatisticsTheme {
	selected: {
		region: RegionTransport["id"] | "all";
		fuels: FuelTypes[];
		departement: Departement["code"] | "all";
		timeInterval: StatsTimeType;
		switches: Record<SelectedSwitches, boolean>;
	};
	raw: Statistic[];
	data: DataType[];
	regions: RegionTransport[];
	departements: Departement[];
}

export type SelectedSwitches = "yAxisFrom0";
