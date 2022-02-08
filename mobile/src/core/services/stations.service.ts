import { fuelStationApi } from "../apis";

export class StationsService {
	getFuelStations(latitude: number, longitude: number, radius: number) {
		return fuelStationApi.getFuelStations(latitude, longitude, radius).then((x) => x.data);
	}
}
