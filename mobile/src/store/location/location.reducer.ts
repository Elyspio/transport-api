import { createSlice } from "@reduxjs/toolkit";
import * as Location from "expo-location";
import { PermissionStatus } from "expo-location";
import { initLocation, setLocation } from "./location.action";

const initialState: State = {
	status: PermissionStatus.UNDETERMINED,
};

type State = {
	data?: Location.LocationObject;
	status: Location.PermissionStatus;
};

const locationSlice = createSlice({
	name: "Location",
	reducers: {},
	initialState,
	extraReducers: ({ addCase }) => {
		addCase(setLocation, (state, action) => {
			state.data = action.payload;
		});
		addCase(initLocation.fulfilled, (state) => {
			state.status = PermissionStatus.GRANTED;
		});
		addCase(initLocation.rejected, (state) => {
			state.status = PermissionStatus.DENIED;
		});
	},
});

export const { reducer } = locationSlice;
