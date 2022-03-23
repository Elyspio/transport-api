import { fuelStationApi } from "../apis";

export class StationsService {
	getFuelStations(latitude: number, longitude: number, radius: number) {
		return fuelStationApi.getFuelStationsNear(latitude, longitude, radius).then((x) => x.data);
	}
}
