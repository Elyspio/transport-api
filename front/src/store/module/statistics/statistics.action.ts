import { createAction as _createAction, createAsyncThunk } from "@reduxjs/toolkit";
import { container } from "../../../core/di";
import { StatisticsService } from "../../../core/services/statistics.service";
import { City, Departement, Region, StatsTimeType } from "../../../core/apis/backend/generated";
import { FuelTypes, SelectedSwitches } from "./statistics.types";
import { LocationsService } from "../../../core/services/locations.service";

export const priceTypes = ["e85", "e10", "gazole", "sp95", "sp98"] as FuelTypes[];

const service = container.get(StatisticsService);

const createAction = <T>(name: string) => _createAction<T>(`statistics/${name}`);

export const setSelectedFuels = createAction<typeof priceTypes>("setSelectedFuels");
export const setSelectedRegion = createAction<Region["id"] | "all">("setSelectedRegion");
export const setSelectedDepartement = createAction<Departement["code"] | "all">("setSelectedDepartement");
export const setSelectedCity = createAction<City["postalCode"] | "all">("setSelectedCity");
export const setSelectedTimeInterval = createAction<StatsTimeType>("setSelectedTimeInterval");

export const toggleSwitch = createAction<SelectedSwitches>("toggleSwitch");

export const getStatistics = createAsyncThunk("statistics/getStatistics", async (type: StatsTimeType) => {
	return await service.getWeeklyStats(type);
});

export const getLocations = createAsyncThunk("statistics/getLocations", async () => {
	const service = container.get(LocationsService);
	return await service.getLocations();
});
