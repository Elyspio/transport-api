import React from "react";
import { FormControl, FormControlLabel, Grid, InputLabel, MenuItem, Select, Switch, Typography } from "@mui/material";
import {
	priceTypes,
	setDepartements,
	setRegions,
	setSelectedDepartement,
	setSelectedFuels,
	setSelectedRegion,
	setSelectedTimeInterval,
	toggleSwitch,
} from "../../../../store/module/statistics/statistics.action";
import { Departement, StatsTimeType } from "../../../../core/apis/backend/generated";
import { useAppDispatch, useAppSelector } from "../../../../store";
import { bindActionCreators } from "redux";
import { useInjection } from "inversify-react";
import { LocationsService } from "../../../../core/services/locations.service";
import { useAsyncEffect } from "../../../hooks/useAsyncEffect";
import { SelectedSwitches } from "../../../../store/module/statistics/statistics.types";
import { fuelsLabels, timeLabels, timeOrdered } from "./StatControls.constants";

export function StatControls() {
	const {
		selected: { fuels, region, timeInterval, departement, switches },
		regions,
		departements,
	} = useAppSelector((s) => s.statistic);

	const dispatch = useAppDispatch();

	const services = {
		locations: useInjection(LocationsService),
	};

	const update = React.useMemo(
		() =>
			bindActionCreators(
				{
					setSelectedRegion,
					setSelectedFuels,
					setSelectedTimeInterval,
					setSelectedDepartement,
					setDepartements,
					setRegions,
					toggleSwitch,
				},
				dispatch
			),
		[dispatch]
	);

	useAsyncEffect(async () => {
		let departements: Departement[];
		if (region === "all") {
			departements = await services.locations.getAllDepartements();
		} else {
			departements = await services.locations.getDepartements(region);
		}
		update.setDepartements(departements);
	}, [services.locations, region, update]);

	useAsyncEffect(async () => {
		const regions = await services.locations.getRegions();
		update.setRegions(regions);
	}, [services.locations, update]);

	const onRegionSelected = React.useCallback(
		(e) => {
			update.setSelectedRegion(regions.find((region) => region.id === e.target.value)?.id ?? "all");
		},
		[update, regions]
	);
	const onDepartementSelected = React.useCallback(
		(e) => {
			update.setSelectedDepartement(departements.find((region) => region.code === e.target.value)?.code ?? "all");
		},
		[update, departements]
	);

	const onSwitchSelected = React.useCallback(
		(sw: SelectedSwitches) => () => {
			update.toggleSwitch(sw);
		},
		[update]
	);

	return (
		<Grid container direction={"column"} spacing={4} p={2}>
			<Grid item>
				<FormControl fullWidth>
					<InputLabel id="fuel-label">Fuels</InputLabel>
					<Select fullWidth multiple labelId="fuels-label" id="fuels-select" value={fuels} label="Fuels" onChange={(e) => update.setSelectedFuels(e.target.value as any)}>
						{priceTypes.map((val) => (
							<MenuItem value={val} key={val}>
								{fuelsLabels[val]}
							</MenuItem>
						))}
					</Select>
				</FormControl>
			</Grid>

			<Grid item>
				<FormControl fullWidth>
					<InputLabel id="time-interval-label">Depuis</InputLabel>
					<Select
						labelId="time-interval-label"
						id="time-interval-select"
						value={timeInterval}
						label="Time Interval"
						fullWidth
						onChange={(e) => update.setSelectedTimeInterval(e.target.value as StatsTimeType)}
					>
						{timeOrdered.map((fuel) => (
							<MenuItem value={fuel} key={fuel}>
								{timeLabels[fuel]}
							</MenuItem>
						))}
					</Select>
				</FormControl>
			</Grid>
			<Grid item>
				<FormControl fullWidth>
					<InputLabel id="demo-region-label">Région</InputLabel>
					<Select fullWidth labelId="demo-region-label" id="demo-region-select" value={region} label="Région" onChange={onRegionSelected}>
						<MenuItem value={"all"}>All</MenuItem>
						{regions.map((region) => (
							<MenuItem key={region.id} value={region.id}>
								{region.label}
							</MenuItem>
						))}
					</Select>
				</FormControl>
			</Grid>

			<Grid item>
				<FormControl fullWidth>
					<InputLabel id="demo-region-label">Département</InputLabel>
					<Select fullWidth labelId="demo-region-label" id="demo-region-select" value={departement} label="Département" onChange={onDepartementSelected}>
						<MenuItem value={"all"}>All</MenuItem>
						{departements.map((departement) => (
							<MenuItem key={departement.code} value={departement.code}>
								{departement.name} ({departement.code})
							</MenuItem>
						))}
					</Select>
				</FormControl>
			</Grid>

			<Grid item>
				<FormControlLabel
					labelPlacement={"start"}
					control={<Switch value={!switches.yAxisFrom0} color={"primary"} onChange={onSwitchSelected("yAxisFrom0")} />}
					label={<Typography variant={"overline"}>Start prices at 0</Typography>}
				/>
			</Grid>
		</Grid>
	);
}
