import React from "react";
import { FormControl, Grid, InputLabel, MenuItem, Select } from "@mui/material";
import { priceTypes, setSelectedFuels, setSelectedRegion, setSelectedTimeInterval } from "../../../store/module/statistics/statistics.action";
import { StatsTimeType } from "../../../core/apis/backend/generated";
import { useAppDispatch, useAppSelector } from "../../../store";
import { bindActionCreators } from "redux";

function StatControls() {
	const {
		selected: { fuels, region, timeInterval },
		regions,
	} = useAppSelector((s) => s.statistic);

	const dispatch = useAppDispatch();

	const update = React.useMemo(
		() =>
			bindActionCreators(
				{
					setSelectedRegion,
					setSelectedFuels,
					setSelectedTimeInterval,
				},
				dispatch
			),
		[dispatch]
	);

	return (
		<Grid container direction={"column"} spacing={4} p={2}>
			<Grid item>
				<FormControl fullWidth>
					<InputLabel id="fuel-label">Fuels</InputLabel>
					<Select fullWidth multiple labelId="fuels-label" id="fuels-select" value={fuels} label="Fuels" onChange={(e) => update.setSelectedFuels(e.target.value as any)}>
						{priceTypes.map((val) => (
							<MenuItem value={val} key={val}>
								{val}
							</MenuItem>
						))}
					</Select>
				</FormControl>
			</Grid>

			<Grid item>
				<FormControl fullWidth>
					<InputLabel id="time-interval-label">Time Interval</InputLabel>
					<Select
						labelId="time-interval-label"
						id="time-interval-select"
						value={timeInterval}
						label="Time Interval"
						fullWidth
						onChange={(e) => update.setSelectedTimeInterval(e.target.value as StatsTimeType)}
					>
						{Object.entries(StatsTimeType).map(([key, val]) => (
							<MenuItem value={val} key={val}>
								{key}
							</MenuItem>
						))}
					</Select>
				</FormControl>
			</Grid>
			<Grid item>
				<FormControl fullWidth>
					<InputLabel id="demo-region-label">Region</InputLabel>
					<Select fullWidth labelId="demo-region-label" id="demo-region-select" value={region} label="Region" onChange={(e) => update.setSelectedRegion(e.target.value)}>
						<MenuItem value={"all"}>All</MenuItem>
						{regions.map((region) => (
							<MenuItem key={region} value={region}>
								{region}
							</MenuItem>
						))}
					</Select>
				</FormControl>
			</Grid>
		</Grid>
	);
}

export default StatControls;
