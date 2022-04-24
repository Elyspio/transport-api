import { createReducer } from "@reduxjs/toolkit";
import {
	getStatistics,
	setDepartements,
	setRegions,
	setSelectedDepartement,
	setSelectedFuels,
	setSelectedRegion,
	setSelectedTimeInterval,
	toggleSwitch,
} from "./statistics.action";
import { Statistic, StatsTimeType } from "../../../core/apis/backend/generated";
import { DataType, StatisticsTheme } from "./statistics.types";

const defaultState: StatisticsTheme = {
	raw: [],
	data: [],
	selected: {
		departement: "all",
		region: "all",
		fuels: ["gazole", "e10"],
		timeInterval: StatsTimeType.Month3,
		switches: {
			yAxisFrom0: false,
		},
	},
	regions: [],
	departements: [],
};

export const statisticsReducer = createReducer(defaultState, ({ addCase }) => {
	function getRegion(
		stat: Statistic,
		fuels: StatisticsTheme["selected"]["fuels"],
		region: StatisticsTheme["selected"]["region"],
		department: StatisticsTheme["selected"]["departement"]
	) {
		const newData = {
			date: new Date(stat.time).toLocaleDateString(),
		} as DataType;

		if (department !== "all") {
			Object.entries(stat.data.departements[department].average).forEach(([fuel, val]) => {
				if (fuels.map((x) => x.toLocaleLowerCase()).includes(fuel.toLocaleLowerCase())) {
					newData[fuel.toLocaleLowerCase()] = val;
				}
			});
		} else if (region !== "all") {
			Object.entries(stat.data.regions[region].average).forEach(([fuel, val]) => {
				if (fuels.map((x) => x.toLocaleLowerCase()).includes(fuel.toLocaleLowerCase())) {
					newData[fuel.toLocaleLowerCase()] = val;
				}
			});
		}

		return newData;
	}

	function updateData(state: StatisticsTheme) {
		const {
			selected: { fuels, region: selectedRegion, departement },
			raw,
			regions,
		} = state;
		let arr = [] as DataType[];
		const statsSorted = [...raw].sort((s1, s2) => (new Date(s1.time).getTime() < new Date(s2.time).getTime() ? -1 : 1));

		statsSorted.forEach((stat) => {
			let newData: DataType;

			if (selectedRegion !== "all") {
				newData = getRegion(stat, fuels, selectedRegion, departement);
			} else {
				let newDatas: DataType[] = [];
				regions.forEach((region) => newDatas.push(getRegion(stat, fuels, region.id, departement)));
				newDatas = newDatas.filter((data) => fuels.every((fuel) => Object.keys(data).includes(fuel)));

				newData = {
					date: new Date(stat.time).toLocaleDateString(),
				} as DataType;

				fuels.forEach((fuel) => {
					newData[fuel] = newDatas.reduce((acc, current) => acc + current[fuel], 0) / newDatas.length;
				});
			}

			if (!arr.find((stat) => stat.date === newData.date)) {
				arr.push(newData);
			}
		});
		//
		// if(departement) {
		// 	arr = arr.filter(data  => data.)
		// }

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
		updateData(state);
	});

	addCase(setSelectedRegion, (state, { payload }) => {
		state.selected.region = payload;
		state.selected.departement = "all";
		updateData(state);
	});

	addCase(setSelectedDepartement, (state, { payload }) => {
		state.selected.departement = payload;
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

	addCase(setDepartements, (state, { payload }) => {
		state.departements = payload.sort((dep1, dep2) => dep1.name.localeCompare(dep2.name));
		updateData(state);
	});

	addCase(setRegions, (state, { payload }) => {
		state.regions = payload.sort((dep1, dep2) => (parseInt(dep1.code) < parseInt(dep2.code) ? -1 : 1));
	});

	addCase(toggleSwitch, (state, { payload }) => {
		state.selected.switches[payload] = !state.selected.switches[payload];
	});
});
