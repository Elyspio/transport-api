import { createAction, createAsyncThunk } from "@reduxjs/toolkit";
import * as Location from "expo-location";
import { PermissionStatus } from "expo-location";

export const initLocation = createAsyncThunk("location/initLocation", async (_, { dispatch }) => {
	let { status } = await Location.requestForegroundPermissionsAsync();
	if (status === PermissionStatus.GRANTED) {
		const getLocation = async () => {
			const location = await Location.getCurrentPositionAsync({});
			dispatch(setLocation(location));
		};
		setInterval(getLocation, 5000);
		await getLocation();
	}
});

export const setLocation = createAction<Location.LocationObject>("location/setLocation");
