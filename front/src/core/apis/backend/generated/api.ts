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
 * @interface FuelStationDataDistance
 */
export interface FuelStationDataDistance {
	/**
	 *
	 * @type {number}
	 * @memberof FuelStationDataDistance
	 */
	"id": number;
	/**
	 *
	 * @type {Location}
	 * @memberof FuelStationDataDistance
	 */
	"location": Location;
	/**
	 *
	 * @type {Prices}
	 * @memberof FuelStationDataDistance
	 */
	"prices": Prices;
	/**
	 *
	 * @type {Array<FuelStationServiceType>}
	 * @memberof FuelStationDataDistance
	 */
	"services": Array<FuelStationServiceType>;
	/**
	 *
	 * @type {number}
	 * @memberof FuelStationDataDistance
	 */
	"distance": number;
}

/**
 *
 * @export
 * @enum {string}
 */

export enum FuelStationServiceType {
	AireDeCampingCars = "AireDeCampingCars",
	AutomateCb2424 = "AutomateCb2424",
	Bar = "Bar",
	Borneslectriques = "BornesÉlectriques",
	BoutiqueAlimentaire = "BoutiqueAlimentaire",
	BoutiqueNonAlimentaire = "BoutiqueNonAlimentaire",
	CarburantAdditiv = "CarburantAdditivé",
	DabDistributeurAutomatiqueDeBillets = "DabDistributeurAutomatiqueDeBillets",
	Douches = "Douches",
	EspaceBb = "EspaceBébé",
	Gnv = "Gnv",
	LavageAutomatique = "LavageAutomatique",
	LavageManuel = "LavageManuel",
	Laverie = "Laverie",
	LocationDeVhicule = "LocationDeVéhicule",
	PistePoidsLourds = "PistePoidsLourds",
	RelaisColis = "RelaisColis",
	RestaurationSurPlace = "RestaurationSurPlace",
	RestaurationEmporter = "RestaurationÀEmporter",
	ServicesRparationEntretien = "ServicesRéparationEntretien",
	StationDeGonflage = "StationDeGonflage",
	ToilettesPubliques = "ToilettesPubliques",
	VenteDAdditifsCarburants = "VenteDAdditifsCarburants",
	VenteDeFioulDomestique = "VenteDeFioulDomestique",
	VenteDeGazDomestiqueButanePropane = "VenteDeGazDomestiqueButanePropane",
	VenteDePtroleLampant = "VenteDePétroleLampant",
	Wifi = "Wifi"
}

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
	"latitude": number;
	/**
	 *
	 * @type {number}
	 * @memberof Location
	 */
	"longitude": number;
	/**
	 *
	 * @type {string}
	 * @memberof Location
	 */
	"postalCode": string;
	/**
	 *
	 * @type {string}
	 * @memberof Location
	 */
	"address": string;
	/**
	 *
	 * @type {string}
	 * @memberof Location
	 */
	"city": string;
}

/**
 *
 * @export
 * @interface Prices
 */
export interface Prices {
	/**
	 *
	 * @type {Array<number>}
	 * @memberof Prices
	 */
	"e10": Array<number>;
	/**
	 *
	 * @type {Array<number>}
	 * @memberof Prices
	 */
	"e85": Array<number>;
	/**
	 *
	 * @type {Array<number>}
	 * @memberof Prices
	 */
	"gazole": Array<number>;
	/**
	 *
	 * @type {Array<number>}
	 * @memberof Prices
	 */
	"gpLc": Array<number>;
	/**
	 *
	 * @type {Array<number>}
	 * @memberof Prices
	 */
	"sp95": Array<number>;
	/**
	 *
	 * @type {Array<number>}
	 * @memberof Prices
	 */
	"sp98": Array<number>;
}

/**
 * FuelStationsApi - axios parameter creator
 * @export
 */
export const FuelStationsApiAxiosParamCreator = function(configuration?: Configuration) {
	return {
		/**
		 *
		 * @param {number} latitude
		 * @param {number} longitude
		 * @param {number} [radius]
		 * @param {*} [options] Override http request option.
		 * @throws {RequiredError}
		 */
		getFuelStations: async (latitude: number, longitude: number, radius?: number, options: AxiosRequestConfig = {}): Promise<RequestArgs> => {
			// verify required parameter 'latitude' is not null or undefined
			assertParamExists("getFuelStations", "latitude", latitude);
			// verify required parameter 'longitude' is not null or undefined
			assertParamExists("getFuelStations", "longitude", longitude);
			const localVarPath = `/api/fuel-stations`;
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
export const FuelStationsApiFp = function(configuration?: Configuration) {
	const localVarAxiosParamCreator = FuelStationsApiAxiosParamCreator(configuration);
	return {
		/**
		 *
		 * @param {number} latitude
		 * @param {number} longitude
		 * @param {number} [radius]
		 * @param {*} [options] Override http request option.
		 * @throws {RequiredError}
		 */
		async getFuelStations(latitude: number, longitude: number, radius?: number, options?: AxiosRequestConfig): Promise<(axios?: AxiosInstance, basePath?: string) => AxiosPromise<Array<FuelStationDataDistance>>> {
			const localVarAxiosArgs = await localVarAxiosParamCreator.getFuelStations(latitude, longitude, radius, options);
			return createRequestFunction(localVarAxiosArgs, globalAxios, BASE_PATH, configuration);
		},
	};
};

/**
 * FuelStationsApi - factory interface
 * @export
 */
export const FuelStationsApiFactory = function(configuration?: Configuration, basePath?: string, axios?: AxiosInstance) {
	const localVarFp = FuelStationsApiFp(configuration);
	return {
		/**
		 *
		 * @param {number} latitude
		 * @param {number} longitude
		 * @param {number} [radius]
		 * @param {*} [options] Override http request option.
		 * @throws {RequiredError}
		 */
		getFuelStations(latitude: number, longitude: number, radius?: number, options?: any): AxiosPromise<Array<FuelStationDataDistance>> {
			return localVarFp.getFuelStations(latitude, longitude, radius, options).then((request) => request(axios, basePath));
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
	 * @param {number} latitude
	 * @param {number} longitude
	 * @param {number} [radius]
	 * @param {*} [options] Override http request option.
	 * @throws {RequiredError}
	 * @memberof FuelStationsApi
	 */
	public getFuelStations(latitude: number, longitude: number, radius?: number, options?: AxiosRequestConfig) {
		return FuelStationsApiFp(this.configuration).getFuelStations(latitude, longitude, radius, options).then((request) => request(this.axios, this.basePath));
	}
}


