import React from "react";
import { Autocomplete, FormControl, FormControlLabel, InputLabel, MenuItem, Select, SelectChangeEvent, Stack, Switch, TextField, Typography } from "@mui/material";
import {
	getLocations,
	priceTypes,
	setSelectedCity,
	setSelectedDepartement,
	setSelectedFuels,
	setSelectedRegion,
	setSelectedTimeInterval,
	toggleSwitch,
} from "../../../../store/module/statistics/statistics.action";
import { City, Departement, Region, StatsTimeType } from "../../../../core/apis/backend/generated";
import { useAppDispatch, useAppSelector } from "../../../../store";
import { bindActionCreators } from "redux";
import { useInjection } from "inversify-react";
import { LocationsService } from "../../../../core/services/locations.service";
import { useAsyncEffect } from "../../../hooks/useAsyncEffect";
import { fuelsLabels, timeLabels, timeOrdered } from "./StatControls.constants";
import { SelectedSwitches } from "../../../../store/module/statistics/statistics.types";

export function StatControls() {
	const {
		selected: { fuels, region, timeInterval, departement, switches, city },
		locations,
		departements,
		cities,
	} = useAppSelector((s) => s.statistic);

	const dispatch = useAppDispatch();

	const services = {
		locations: useInjection(LocationsService),
	};

	const update = React.useMemo(
		() =>
			bindActionCreators(
				{
					getLocations,
					setSelectedRegion,
					setSelectedFuels,
					setSelectedCity,
					setSelectedTimeInterval,
					setSelectedDepartement,
					toggleSwitch,
				},
				dispatch
			),
		[dispatch]
	);

	useAsyncEffect(async () => {
		await update.getLocations();
	}, [services.locations, update]);

	const onSelected = React.useCallback(
		(type: "region" | "city" | "departement") => (e: SelectChangeEvent) => {
			const funcs: Record<typeof type, any> = {
				city: update.setSelectedCity,
				departement: update.setSelectedDepartement,
				region: update.setSelectedRegion,
			};
			funcs[type](e.target.value);
		},
		[update]
	);

	const onCitySelected = React.useCallback(
		(e: any, city: City | null) => {
			update.setSelectedCity(city?.postalCode ?? "all");
		},
		[update]
	);

	const onDepartementSelected = React.useCallback(
		(e: any, city: Departement | null) => {
			update.setSelectedDepartement(city?.code ?? "all");
		},
		[update]
	);
	const onRegionSelected = React.useCallback(
		(e: any, region: Region | null) => {
			update.setSelectedRegion(region?.id ?? "all");
		},
		[update]
	);

	const onSwitchSelected = React.useCallback(
		(sw: SelectedSwitches) => () => {
			update.toggleSwitch(sw);
		},
		[update]
	);

	return (
		<Stack direction={"column"} spacing={4} p={2}>
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

			<FormControl fullWidth>
				<InputLabel id="time-interval-label">Depuis</InputLabel>
				<Select
					labelId="time-interval-label"
					id="time-interval-select"
					value={timeInterval}
					label="Depuis"
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

			<FormControl fullWidth>
				<Autocomplete
					disablePortal
					fullWidth
					id="select-region"
					options={locations}
					getOptionLabel={(option) => `${option.label}`}
					onChange={onRegionSelected}
					renderInput={(params) => <TextField {...params} autoComplete="off" data-lpignore="true" data-form-type="other" label="Régions" />}
				/>
			</FormControl>

			<FormControl fullWidth>
				<Autocomplete
					disablePortal
					fullWidth
					id="select-departement"
					disabled={region === "all"}
					options={departements}
					getOptionLabel={(option) => `${option.name} (${option.code})`}
					onChange={onDepartementSelected}
					renderInput={(params) => <TextField {...params} autoComplete="off" data-lpignore="true" data-form-type="other" label="Département" />}
				/>
			</FormControl>

			<FormControl fullWidth>
				<Autocomplete
					disablePortal
					fullWidth
					disabled={departement === "all"}
					id="select-city"
					options={cities}
					getOptionLabel={(option) => `${option.name} ${option.postalCode}`}
					onChange={onCitySelected}
					renderInput={(params) => <TextField {...params} autoComplete="off" data-lpignore="true" data-form-type="other" label="Commune" />}
				/>
			</FormControl>

			<FormControl fullWidth>
				<FormControlLabel
					labelPlacement={"start"}
					control={<Switch value={!switches.yAxisFrom0} sx={{ marginLeft: "auto", marginRight: "1rem" }} color={"primary"} onChange={onSwitchSelected("yAxisFrom0")} />}
					label={<Typography variant={"overline"}>Échelle à 0 </Typography>}
				/>
			</FormControl>
		</Stack>
	);
}
