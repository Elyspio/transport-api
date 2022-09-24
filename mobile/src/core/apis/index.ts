import { FuelStationsApi } from "./backend/generated";
import axios from "axios";

const url = "https://elyspio.fr/transport";

const instance = axios.create({
	baseURL: url,
	timeout: 1000,
});

export const fuelStationApi = new FuelStationsApi(undefined, url, instance);
