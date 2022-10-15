import { Prices } from "../../core/apis/backend/generated";

export const fuelsLabels: Record<PriceValues, string> = {
	e10: "E10",
	e85: "E85",
	gazole: "Gazole",
	gpLc: "GpLc",
	sp95: "SP95",
	sp98: "SP98",
};

export type PriceValues = keyof Prices;
