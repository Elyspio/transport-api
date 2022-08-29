import { inject, injectable } from "inversify";
import { Log } from "../utils/decorators/logger";
import { getLogger } from "../utils/logger";
import { BackendApiClient } from "../apis/backend";
import { BaseService } from "./base.service";

@injectable()
export class LocationsService extends BaseService {
	private logger = getLogger.service(LocationsService);

	@inject(BackendApiClient)
	private client!: BackendApiClient;

	@Log.service()
	getLocations() {
		return this.client.clients.locations.getAll().then(this.extractData);
	}
}
