import { createSlice } from "@reduxjs/toolkit";
import { getFuelStations } from "./stations.action";
import { FuelStationDataDistance } from "../../core/apis/backend/generated";

const initialState: State = {
	data: [],
};

type State = {
	data: FuelStationDataDistance[];
};

const locationSlice = createSlice({
	name: "Stations",
	reducers: {},
	initialState,
	extraReducers: ({ addCase }) => {
		addCase(getFuelStations.fulfilled, (state, { payload }) => {
			state.data = payload;
		});
	},
});

export const { reducer } = locationSlice;
