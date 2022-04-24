import { Departement, Prices, RegionTransport, Statistic, StatsTimeType } from "../../../core/apis/backend/generated";

export type PriceTypes = keyof Prices;
export type DataType = Record<PriceTypes, number> & {
	date: string;
};

export interface StatisticsTheme {
	selected: {
		region: RegionTransport["id"] | "all";
		fuels: PriceTypes[];
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
