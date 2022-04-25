import { createAction as _createAction, createAsyncThunk } from "@reduxjs/toolkit";
import { container } from "../../../core/di";
import { StatisticsService } from "../../../core/services/statistics.service";
import { Departement, RegionTransport, StatsTimeType } from "../../../core/apis/backend/generated";
import { FuelTypes, SelectedSwitches } from "./statistics.types";

export const priceTypes = ["e85", "e10", "gazole", "sp95", "sp98"] as FuelTypes[];

const service = container.get(StatisticsService);

const createAction = <T>(name: string) => _createAction<T>(`statistics/${name}`);

export const setSelectedFuels = createAction<typeof priceTypes>("setSelectedFuels");

export const setSelectedRegion = createAction<RegionTransport["id"] | "all">("setSelectedRegion");
export const setSelectedDepartement = createAction<Departement["code"] | "all">("setSelectedDepartement");
export const setSelectedTimeInterval = createAction<StatsTimeType>("setSelectedTimeInterval");
export const setDepartements = createAction<Departement[]>("setDepartements");
export const setRegions = createAction<RegionTransport[]>("setRegions");

export const toggleSwitch = createAction<SelectedSwitches>("toggleSwitch");

export const getStatistics = createAsyncThunk("statistics/getStatistics", async (type: StatsTimeType) => {
	return await service.getWeeklyStats(type);
});
