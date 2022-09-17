import { createReducer } from "@reduxjs/toolkit";
import {
	getLocations,
	getStatistics,
	setSelectedCity,
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
		city: "all",
		switches: {
			yAxisFrom0: false,
		},
	},
	cities: [],
	locations: [],
	departements: [],
};

export const statisticsReducer = createReducer(defaultState, ({ addCase }) => {
	function getRegion(
		stat: Statistic,
		fuels: StatisticsTheme["selected"]["fuels"],
		region: StatisticsTheme["selected"]["region"],
		department: StatisticsTheme["selected"]["departement"],
		city: StatisticsTheme["selected"]["city"]
	) {
		const newData = {
			date: new Date(stat.time).toLocaleDateString(),
		} as DataType;

		if (city !== "all") {
			Object.entries(stat.data.cities[city].average).forEach(([fuel, val]) => {
				if (fuels.map((x) => x.toLocaleLowerCase()).includes(fuel.toLocaleLowerCase())) {
					newData[fuel.toLocaleLowerCase()] = val;
				}
			});
		} else if (department !== "all") {
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
			selected: { fuels, region: selectedRegion, departement, city },
			raw,
			locations,
		} = state;
		let arr = [] as DataType[];
		const statsSorted = [...raw].sort((s1, s2) => (new Date(s1.time).getTime() < new Date(s2.time).getTime() ? -1 : 1));

		statsSorted.forEach((stat) => {
			let newData: DataType;

			if (selectedRegion !== "all") {
				newData = getRegion(stat, fuels, selectedRegion, departement, city);
			} else {
				let newDatas: DataType[] = [];
				locations.forEach((region) => newDatas.push(getRegion(stat, fuels, region.id, departement, city)));
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

	addCase(getLocations.fulfilled, (state, action) => {
		state.locations = action.payload;
		for (const region of state.locations) {
			for (const dep of region.departements) {
				dep.cities.sort((city, city2) => city.name.localeCompare(city2.name));
			}
			region.departements.sort((dep, dep2) => dep.name.localeCompare(dep2.name));
		}
		state.locations.sort((region, region2) => region.label.localeCompare(region2.label));
	});

	addCase(setSelectedTimeInterval, (state, { payload }) => {
		state.selected.timeInterval = payload;
		updateData(state);
	});

	addCase(toggleSwitch, (state, { payload }) => {
		state.selected.switches[payload] = !state.selected.switches[payload];
	});

	addCase(setSelectedCity, (state, action) => {
		state.selected.city = action.payload;
		updateData(state);
	});

	addCase(setSelectedRegion, (state, { payload }) => {
		state.selected.region = payload;
		state.selected.departement = "all";
		state.departements = state.locations.find((loc) => loc.id === payload)?.departements ?? [];
		updateData(state);
	});

	addCase(setSelectedDepartement, (state, { payload }) => {
		state.selected.departement = payload;
		state.selected.city = "all";
		state.cities = state.departements.find((dep) => dep.code === payload)?.cities ?? [];
		updateData(state);
	});

	addCase(setSelectedFuels, (state, { payload }) => {
		state.selected.fuels = payload;
		updateData(state);
	});
});
