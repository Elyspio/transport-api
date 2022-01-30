import { FuelStationsApi } from "./backend/generated";
import axios from "axios";

const url = process.env.NODE_ENV === "development" ? "http://192.168.0.44:4000" : "https://elyspio.fr/transportation";

const instance = axios.create({
	baseURL: url,
	timeout: 1000,
});

export const fuelStationApi = new FuelStationsApi(undefined, url, axios);
