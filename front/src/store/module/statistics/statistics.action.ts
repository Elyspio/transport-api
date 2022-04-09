import { createAction as _createAction, createAsyncThunk } from "@reduxjs/toolkit";
import { container } from "../../../core/di";
import { StatisticsService } from "../../../core/services/statistics.service";
import { Prices, StatsTimeType } from "../../../core/apis/backend/generated";

export type PriceTypes = keyof Prices;
export type DataType = Record<PriceTypes, number> & {
	date: string;
};

export const priceTypes = ["e85", "e10", "gazole", "sp95", "sp98"] as PriceTypes[];

const service = container.get(StatisticsService);

const createAction = <T>(name: string) => _createAction<T>(`statistics/${name}`);

export const setSelectedFuels = createAction<typeof priceTypes>("setSelectedFuels");

export const setSelectedRegion = createAction<string | "all">("setSelectedRegion");
export const setSelectedTimeInterval = createAction<StatsTimeType>("setSelectedTimeInterval");

export const getStatistics = createAsyncThunk("statistics/getStatistics", async (type: StatsTimeType) => {
	return await service.getWeeklyStats(type);
});
