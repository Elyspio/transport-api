import { createSlice } from "@reduxjs/toolkit";
import { getFuelStationsNow } from "./stations.action";
import { FuelStationDataDistance, Prices } from "../../core/apis/backend/generated";

const initialState: State = {
	now: [],
	lowest: {
		e10: undefined,
		sp98: undefined,
		e85: undefined,
		sp95: undefined,
		gazole: undefined,
		gpLc: undefined,
	},
};

type PriceTypes = keyof Prices;
type State = {
	now: FuelStationDataDistance[];
	lowest: Record<PriceTypes, number | undefined>;
};

const locationSlice = createSlice({
	name: "Stations",
	reducers: {},
	initialState,
	extraReducers: ({ addCase }) => {
		addCase(getFuelStationsNow.fulfilled, (state, { payload }) => {
			state.now = payload;

			// @ts-ignore
			Object.keys(state.lowest).forEach((fuel: PriceTypes) => {
				const lowest = state.now.reduce((prev, current) => {
					let newVal = current.prices[fuel][0]?.value;
					let oldVal = prev.prices[fuel][0]?.value;

					if (!oldVal) return current;

					return newVal < oldVal ? current : prev;
				});
				state.lowest[fuel] = lowest.prices[fuel]?.[0]?.value;
			});
		});
	},
});

export const { reducer } = locationSlice;
