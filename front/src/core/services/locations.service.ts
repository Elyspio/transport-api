import { inject, injectable } from "inversify";
import { Log } from "../utils/decorators/logger";
import { Region } from "../apis/backend/generated";
import { getLogger } from "../utils/logger";
import { BackendApiClient } from "../apis/backend";
import { BaseService } from "./base.service";

@injectable()
export class LocationsService extends BaseService {
	private logger = getLogger.service(LocationsService);

	@inject(BackendApiClient)
	private client!: BackendApiClient;

	@Log.service()
	getRegions() {
		return this.client.clients.locations.getRegions().then(this.extractData);
	}

	@Log.service()
	getDepartements(region: Region) {
		return this.client.clients.locations.getDepartementsByRegion(region).then(this.extractData);
	}

	@Log.service()
	getAllDepartements() {
		return this.client.clients.locations.getAllDepartements().then(this.extractData);
	}
}
