import { LocationService } from "./location";
import { StationsService } from "./stations.service";

export const Services = {
	location: new LocationService(),
	stations: new StationsService(),
};
