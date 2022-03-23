/* tslint:disable */
/* eslint-disable */
/**
 * Web
 * No description provided (generated by Openapi Generator https://github.com/openapitools/openapi-generator)
 *
 * The version of the OpenAPI document: 1.0
 *
 *
 * NOTE: This class is auto generated by OpenAPI Generator (https://openapi-generator.tech).
 * https://openapi-generator.tech
 * Do not edit the class manually.
 */

import { Configuration } from "./configuration";
import globalAxios, { AxiosInstance, AxiosPromise, AxiosRequestConfig } from "axios";
// Some imports not used depending on template conditions
// @ts-ignore
import {
	assertParamExists,
	createRequestFunction,
	DUMMY_BASE_URL,
	serializeDataIfNeeded,
	setApiKeyToObject,
	setBasicAuthToObject,
	setBearerAuthToObject,
	setOAuthToObject,
	setSearchParams,
	toPathString,
} from "./common";
// @ts-ignore
import { BASE_PATH, BaseAPI, COLLECTION_FORMATS, RequestArgs, RequiredError } from "./base";

/**
 *
 * @export
 * @interface FuelPriceHistory
 */
export interface FuelPriceHistory {
	/**
	 *
	 * @type {string}
	 * @memberof FuelPriceHistory
	 */
	date: string;
	/**
	 *
	 * @type {number}
	 * @memberof FuelPriceHistory
	 */
	value: number;
}

/**
 *
 * @export
 * @interface FuelStationData
 */
export interface FuelStationData {
	/**
	 *
	 * @type {number}
	 * @memberof FuelStationData
	 */
	id: number;
	/**
	 *
	 * @type {Location}
	 * @memberof FuelStationData
	 */
	location: Location;
	/**
	 *
	 * @type {Prices}
	 * @memberof FuelStationData
	 */
	prices: Prices;
	/**
	 *
	 * @type {Array<FuelStationServiceType>}
	 * @memberof FuelStationData
	 */
	services: Array<FuelStationServiceType>;
}

/**
 *
 * @export
 * @interface FuelStationDataDistance
 */
export interface FuelStationDataDistance {
	/**
	 *
	 * @type {number}
	 * @memberof FuelStationDataDistance
	 */
	id: number;
	/**
	 *
	 * @type {Location}
	 * @memberof FuelStationDataDistance
	 */
	location: Location;
	/**
	 *
	 * @type {Prices}
	 * @memberof FuelStationDataDistance
	 */
	prices: Prices;
	/**
	 *
	 * @type {Array<FuelStationServiceType>}
	 * @memberof FuelStationDataDistance
	 */
	services: Array<FuelStationServiceType>;
	/**
	 *
	 * @type {number}
	 * @memberof FuelStationDataDistance
	 */
	distance: number;
}

/**
 *
 * @export
 * @interface FuelStationHistory
 */
export interface FuelStationHistory {
	/**
	 *
	 * @type {number}
	 * @memberof FuelStationHistory
	 */
	id?: number;
	/**
	 *
	 * @type {Location}
	 * @memberof FuelStationHistory
	 */
	location?: Location;
	/**
	 *
	 * @type {{ [key: string]: Array<FuelPriceHistory>; }}
	 * @memberof FuelStationHistory
	 */
	prices?: { [key: string]: Array<FuelPriceHistory> } | null;
}

/**
 *
 * @export
 * @enum {string}
 */

export const FuelStationServiceType = {
	AireDeCampingCars: "AireDeCampingCars",
	AutomateCb2424: "AutomateCb2424",
	Bar: "Bar",
	Borneslectriques: "BornesÉlectriques",
	BoutiqueAlimentaire: "BoutiqueAlimentaire",
	BoutiqueNonAlimentaire: "BoutiqueNonAlimentaire",
	CarburantAdditiv: "CarburantAdditivé",
	DabDistributeurAutomatiqueDeBillets: "DabDistributeurAutomatiqueDeBillets",
	Douches: "Douches",
	EspaceBb: "EspaceBébé",
	Gnv: "Gnv",
	LavageAutomatique: "LavageAutomatique",
	LavageManuel: "LavageManuel",
	Laverie: "Laverie",
	LocationDeVhicule: "LocationDeVéhicule",
	PistePoidsLourds: "PistePoidsLourds",
	RelaisColis: "RelaisColis",
	RestaurationSurPlace: "RestaurationSurPlace",
	RestaurationEmporter: "RestaurationÀEmporter",
	ServicesRparationEntretien: "ServicesRéparationEntretien",
	StationDeGonflage: "StationDeGonflage",
	ToilettesPubliques: "ToilettesPubliques",
	VenteDAdditifsCarburants: "VenteDAdditifsCarburants",
	VenteDeFioulDomestique: "VenteDeFioulDomestique",
	VenteDeGazDomestiqueButanePropane: "VenteDeGazDomestiqueButanePropane",
	VenteDePtroleLampant: "VenteDePétroleLampant",
	Wifi: "Wifi",
} as const;

export type FuelStationServiceType = typeof FuelStationServiceType[keyof typeof FuelStationServiceType];

/**
 *
 * @export
 * @interface Location
 */
export interface Location {
	/**
	 *
	 * @type {number}
	 * @memberof Location
	 */
	latitude: number;
	/**
	 *
	 * @type {number}
	 * @memberof Location
	 */
	longitude: number;
	/**
	 *
	 * @type {string}
	 * @memberof Location
	 */
	postalCode: string;
	/**
	 *
	 * @type {string}
	 * @memberof Location
	 */
	address: string;
	/**
	 *
	 * @type {string}
	 * @memberof Location
	 */
	city: string;
}

/**
 *
 * @export
 * @interface Prices
 */
export interface Prices {
	/**
	 *
	 * @type {Array<FuelPriceHistory>}
	 * @memberof Prices
	 */
	e10: Array<FuelPriceHistory>;
	/**
	 *
	 * @type {Array<FuelPriceHistory>}
	 * @memberof Prices
	 */
	e85: Array<FuelPriceHistory>;
	/**
	 *
	 * @type {Array<FuelPriceHistory>}
	 * @memberof Prices
	 */
	gazole: Array<FuelPriceHistory>;
	/**
	 *
	 * @type {Array<FuelPriceHistory>}
	 * @memberof Prices
	 */
	gpLc: Array<FuelPriceHistory>;
	/**
	 *
	 * @type {Array<FuelPriceHistory>}
	 * @memberof Prices
	 */
	sp95: Array<FuelPriceHistory>;
	/**
	 *
	 * @type {Array<FuelPriceHistory>}
	 * @memberof Prices
	 */
	sp98: Array<FuelPriceHistory>;
}

/**
 * FuelStationsApi - axios parameter creator
 * @export
 */
export const FuelStationsApiAxiosParamCreator = function (configuration?: Configuration) {
	return {
		/**
		 *
		 * @param {*} [options] Override http request option.
		 * @throws {RequiredError}
		 */
		fetch: async (options: AxiosRequestConfig = {}): Promise<RequestArgs> => {
			const localVarPath = `/api/fuel-stations/fetch`;
			// use dummy base URL string because the URL constructor only accepts absolute URLs.
			const localVarUrlObj = new URL(localVarPath, DUMMY_BASE_URL);
			let baseOptions;
			if (configuration) {
				baseOptions = configuration.baseOptions;
			}

			const localVarRequestOptions = { method: "GET", ...baseOptions, ...options };
			const localVarHeaderParameter = {} as any;
			const localVarQueryParameter = {} as any;

			setSearchParams(localVarUrlObj, localVarQueryParameter);
			let headersFromBaseOptions = baseOptions && baseOptions.headers ? baseOptions.headers : {};
			localVarRequestOptions.headers = { ...localVarHeaderParameter, ...headersFromBaseOptions, ...options.headers };

			return {
				url: toPathString(localVarUrlObj),
				options: localVarRequestOptions,
			};
		},
		/**
		 *
		 * @param {string} [minDate]
		 * @param {string} [maxDate]
		 * @param {*} [options] Override http request option.
		 * @throws {RequiredError}
		 */
		getFuelStationsBetweenDates: async (minDate?: string, maxDate?: string, options: AxiosRequestConfig = {}): Promise<RequestArgs> => {
			const localVarPath = `/api/fuel-stations/time`;
			// use dummy base URL string because the URL constructor only accepts absolute URLs.
			const localVarUrlObj = new URL(localVarPath, DUMMY_BASE_URL);
			let baseOptions;
			if (configuration) {
				baseOptions = configuration.baseOptions;
			}

			const localVarRequestOptions = { method: "GET", ...baseOptions, ...options };
			const localVarHeaderParameter = {} as any;
			const localVarQueryParameter = {} as any;

			if (minDate !== undefined) {
				localVarQueryParameter["minDate"] = (minDate as any) instanceof Date ? (minDate as any).toISOString() : minDate;
			}

			if (maxDate !== undefined) {
				localVarQueryParameter["maxDate"] = (maxDate as any) instanceof Date ? (maxDate as any).toISOString() : maxDate;
			}

			setSearchParams(localVarUrlObj, localVarQueryParameter);
			let headersFromBaseOptions = baseOptions && baseOptions.headers ? baseOptions.headers : {};
			localVarRequestOptions.headers = { ...localVarHeaderParameter, ...headersFromBaseOptions, ...options.headers };

			return {
				url: toPathString(localVarUrlObj),
				options: localVarRequestOptions,
			};
		},
		/**
		 *
		 * @param {number} latitude
		 * @param {number} longitude
		 * @param {number} [radius]
		 * @param {*} [options] Override http request option.
		 * @throws {RequiredError}
		 */
		getFuelStationsNear: async (latitude: number, longitude: number, radius?: number, options: AxiosRequestConfig = {}): Promise<RequestArgs> => {
			// verify required parameter 'latitude' is not null or undefined
			assertParamExists("getFuelStationsNear", "latitude", latitude);
			// verify required parameter 'longitude' is not null or undefined
			assertParamExists("getFuelStationsNear", "longitude", longitude);
			const localVarPath = `/api/fuel-stations/near`;
			// use dummy base URL string because the URL constructor only accepts absolute URLs.
			const localVarUrlObj = new URL(localVarPath, DUMMY_BASE_URL);
			let baseOptions;
			if (configuration) {
				baseOptions = configuration.baseOptions;
			}

			const localVarRequestOptions = { method: "GET", ...baseOptions, ...options };
			const localVarHeaderParameter = {} as any;
			const localVarQueryParameter = {} as any;

			if (latitude !== undefined) {
				localVarQueryParameter["latitude"] = latitude;
			}

			if (longitude !== undefined) {
				localVarQueryParameter["longitude"] = longitude;
			}

			if (radius !== undefined) {
				localVarQueryParameter["radius"] = radius;
			}

			setSearchParams(localVarUrlObj, localVarQueryParameter);
			let headersFromBaseOptions = baseOptions && baseOptions.headers ? baseOptions.headers : {};
			localVarRequestOptions.headers = { ...localVarHeaderParameter, ...headersFromBaseOptions, ...options.headers };

			return {
				url: toPathString(localVarUrlObj),
				options: localVarRequestOptions,
			};
		},
	};
};

/**
 * FuelStationsApi - functional programming interface
 * @export
 */
export const FuelStationsApiFp = function (configuration?: Configuration) {
	const localVarAxiosParamCreator = FuelStationsApiAxiosParamCreator(configuration);
	return {
		/**
		 *
		 * @param {*} [options] Override http request option.
		 * @throws {RequiredError}
		 */
		async fetch(options?: AxiosRequestConfig): Promise<(axios?: AxiosInstance, basePath?: string) => AxiosPromise<Array<FuelStationHistory>>> {
			const localVarAxiosArgs = await localVarAxiosParamCreator.fetch(options);
			return createRequestFunction(localVarAxiosArgs, globalAxios, BASE_PATH, configuration);
		},
		/**
		 *
		 * @param {string} [minDate]
		 * @param {string} [maxDate]
		 * @param {*} [options] Override http request option.
		 * @throws {RequiredError}
		 */
		async getFuelStationsBetweenDates(
			minDate?: string,
			maxDate?: string,
			options?: AxiosRequestConfig
		): Promise<(axios?: AxiosInstance, basePath?: string) => AxiosPromise<Array<FuelStationData>>> {
			const localVarAxiosArgs = await localVarAxiosParamCreator.getFuelStationsBetweenDates(minDate, maxDate, options);
			return createRequestFunction(localVarAxiosArgs, globalAxios, BASE_PATH, configuration);
		},
		/**
		 *
		 * @param {number} latitude
		 * @param {number} longitude
		 * @param {number} [radius]
		 * @param {*} [options] Override http request option.
		 * @throws {RequiredError}
		 */
		async getFuelStationsNear(
			latitude: number,
			longitude: number,
			radius?: number,
			options?: AxiosRequestConfig
		): Promise<(axios?: AxiosInstance, basePath?: string) => AxiosPromise<Array<FuelStationDataDistance>>> {
			const localVarAxiosArgs = await localVarAxiosParamCreator.getFuelStationsNear(latitude, longitude, radius, options);
			return createRequestFunction(localVarAxiosArgs, globalAxios, BASE_PATH, configuration);
		},
	};
};

/**
 * FuelStationsApi - factory interface
 * @export
 */
export const FuelStationsApiFactory = function (configuration?: Configuration, basePath?: string, axios?: AxiosInstance) {
	const localVarFp = FuelStationsApiFp(configuration);
	return {
		/**
		 *
		 * @param {*} [options] Override http request option.
		 * @throws {RequiredError}
		 */
		fetch(options?: any): AxiosPromise<Array<FuelStationHistory>> {
			return localVarFp.fetch(options).then((request) => request(axios, basePath));
		},
		/**
		 *
		 * @param {string} [minDate]
		 * @param {string} [maxDate]
		 * @param {*} [options] Override http request option.
		 * @throws {RequiredError}
		 */
		getFuelStationsBetweenDates(minDate?: string, maxDate?: string, options?: any): AxiosPromise<Array<FuelStationData>> {
			return localVarFp.getFuelStationsBetweenDates(minDate, maxDate, options).then((request) => request(axios, basePath));
		},
		/**
		 *
		 * @param {number} latitude
		 * @param {number} longitude
		 * @param {number} [radius]
		 * @param {*} [options] Override http request option.
		 * @throws {RequiredError}
		 */
		getFuelStationsNear(latitude: number, longitude: number, radius?: number, options?: any): AxiosPromise<Array<FuelStationDataDistance>> {
			return localVarFp.getFuelStationsNear(latitude, longitude, radius, options).then((request) => request(axios, basePath));
		},
	};
};

/**
 * FuelStationsApi - object-oriented interface
 * @export
 * @class FuelStationsApi
 * @extends {BaseAPI}
 */
export class FuelStationsApi extends BaseAPI {
	/**
	 *
	 * @param {*} [options] Override http request option.
	 * @throws {RequiredError}
	 * @memberof FuelStationsApi
	 */
	public fetch(options?: AxiosRequestConfig) {
		return FuelStationsApiFp(this.configuration)
			.fetch(options)
			.then((request) => request(this.axios, this.basePath));
	}

	/**
	 *
	 * @param {string} [minDate]
	 * @param {string} [maxDate]
	 * @param {*} [options] Override http request option.
	 * @throws {RequiredError}
	 * @memberof FuelStationsApi
	 */
	public getFuelStationsBetweenDates(minDate?: string, maxDate?: string, options?: AxiosRequestConfig) {
		return FuelStationsApiFp(this.configuration)
			.getFuelStationsBetweenDates(minDate, maxDate, options)
			.then((request) => request(this.axios, this.basePath));
	}

	/**
	 *
	 * @param {number} latitude
	 * @param {number} longitude
	 * @param {number} [radius]
	 * @param {*} [options] Override http request option.
	 * @throws {RequiredError}
	 * @memberof FuelStationsApi
	 */
	public getFuelStationsNear(latitude: number, longitude: number, radius?: number, options?: AxiosRequestConfig) {
		return FuelStationsApiFp(this.configuration)
			.getFuelStationsNear(latitude, longitude, radius, options)
			.then((request) => request(this.axios, this.basePath));
	}
}
