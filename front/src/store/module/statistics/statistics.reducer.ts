import { createReducer } from "@reduxjs/toolkit";
import { DataType, getStatistics, PriceTypes, setSelectedFuels, setSelectedRegion, setSelectedTimeInterval } from "./statistics.action";
import { Statistic, StatsTimeType } from "../../../core/apis/backend/generated";

export interface StatisticsTheme {
	selected: {
		region: string | "all";
		fuels: PriceTypes[];
		timeInterval: StatsTimeType;
	};
	raw: Statistic[];
	data: DataType[];
	regions: string[];
}

const defaultState: StatisticsTheme = {
	raw: [],
	data: [],
	selected: {
		region: "all",
		fuels: ["gazole", "e10"],
		timeInterval: StatsTimeType.Month3,
	},
	regions: [],
};

export const statisticsReducer = createReducer(defaultState, ({ addCase }) => {
	function getRegion(stat: Statistic, fuels: StatisticsTheme["regions"], region: string) {
		const newData = {
			date: new Date(stat.time).toLocaleDateString(),
		} as DataType;

		Object.entries(stat.data.regions[region].average).forEach(([fuel, val]) => {
			if (fuels.map((x) => x.toLocaleLowerCase()).includes(fuel.toLocaleLowerCase())) {
				newData[fuel.toLocaleLowerCase()] = val;
			}
		});

		return newData;
	}

	function updateData(state: StatisticsTheme) {
		const {
			selected: { fuels, region: selectedRegion },
			raw,
			regions,
		} = state;
		const arr = [] as DataType[];
		const statsSorted = [...raw].sort((s1, s2) => (new Date(s1.time).getTime() < new Date(s2.time).getTime() ? -1 : 1));

		statsSorted.forEach((stat) => {
			let newData: DataType;

			if (selectedRegion !== "all") {
				newData = getRegion(stat, fuels, selectedRegion);
			} else {
				let newDatas: DataType[] = [];
				regions.forEach((region) => newDatas.push(getRegion(stat, fuels, region)));
				newDatas = newDatas.filter((data) => fuels.every((fuel) => Object.keys(data).includes(fuel)));

				newData = {
					date: new Date(stat.time).toLocaleDateString(),
				} as DataType;

				fuels.forEach((fuel) => {
					newData[fuel] = newDatas.reduce((acc, current) => acc + current[fuel], 0) / newDatas.length;
				});
				console.log({ newDatas, newData });
			}

			if (!arr.find((stat) => stat.date === newData.date)) {
				arr.push(newData);
			}
		});

		state.data = arr;
	}

	addCase(getStatistics.fulfilled, (state, { payload }) => {
		state.raw = payload;
		const regions = new Set<string>();
		state.raw.forEach((stat) => {
			Object.keys(stat.data.regions).forEach((region) => {
				regions.add(region);
			});
		});
		state.regions = [...regions];
		updateData(state);
	});

	addCase(setSelectedRegion, (state, { payload }) => {
		state.selected.region = payload;
		updateData(state);
	});

	addCase(setSelectedFuels, (state, { payload }) => {
		state.selected.fuels = payload;
		updateData(state);
	});

	addCase(setSelectedTimeInterval, (state, { payload }) => {
		state.selected.timeInterval = payload;
		updateData(state);
	});
});
