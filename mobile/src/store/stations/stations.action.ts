import { createAsyncThunk } from "@reduxjs/toolkit";
import { Services } from "../../core/services";

type GetFuelStationsParams = { latitude: number; longitude: number; radius: number };
export const getFuelStations = createAsyncThunk("stations/getFuelStations", async ({ longitude, radius, latitude }: GetFuelStationsParams) => {
	return Services.stations.getFuelStations(latitude, longitude, radius);
});
