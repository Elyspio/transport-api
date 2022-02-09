import { LocationService } from "./location";
import { StationsService } from "./stations.service";
import { WazeService } from "./waze.service";

export const Services = {
	location: new LocationService(),
	stations: new StationsService(),
	waze: new WazeService(),
};
