import { inject, injectable } from "inversify";
import { Log } from "../utils/decorators/logger";
import { StatsTimeType } from "../apis/backend/generated";
import { getLogger } from "../utils/logger";
import { BackendApiClient } from "../apis/backend";
import { BaseService } from "./base.service";

@injectable()
export class StatisticsService extends BaseService {
	private logger = getLogger.service(StatisticsService);

	@inject(BackendApiClient)
	private client!: BackendApiClient;

	@Log.service()
	getWeeklyStats(type: StatsTimeType) {
		return this.client.clients.stats.getWeeklyStats(type).then(this.extractData);
	}
}
