import { LocationService } from "./location";
import { StationsService } from "./stations.service";
import { NavigationService } from "./navigation.service";

export const Services = {
	location: new LocationService(),
	stations: new StationsService(),
	waze: new NavigationService(),
};
