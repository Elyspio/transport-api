import { AxiosResponse } from "axios";
import { injectable } from "inversify";

@injectable()
export class BaseService {
	protected extractData = <T>(response: AxiosResponse<T>): T => response.data;
}
