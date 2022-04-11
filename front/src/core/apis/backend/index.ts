import { injectable } from "inversify";
import axios from "axios";
import { LocationsApi, StatisticsApi } from "./generated";
import { BaseAPI } from "./generated/base";

const instance = axios.create({});

export type Newable<T> = { new (...args: ConstructorParameters<typeof BaseAPI>): T };

function createApi<T extends BaseAPI>(cls: Newable<T>): T {
	return new cls(undefined, window.config.endpoints.core, instance);
}

@injectable()
export class BackendApiClient {
	public clients = {
		stats: createApi(StatisticsApi),
		locations: createApi(LocationsApi),
	};
}
