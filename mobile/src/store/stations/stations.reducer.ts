import { createSlice } from "@reduxjs/toolkit";
import { getFuelStationsNow } from "./stations.action";
import { FuelStationDataDistance } from "../../core/apis/backend/generated";

const initialState: State = {
	now: [],
};

type State = {
	now: FuelStationDataDistance[];
};

const locationSlice = createSlice({
	name: "Stations",
	reducers: {},
	initialState,
	extraReducers: ({ addCase }) => {
		addCase(getFuelStationsNow.fulfilled, (state, { payload }) => {
			state.now = payload;
		});
	},
});

export const { reducer } = locationSlice;
