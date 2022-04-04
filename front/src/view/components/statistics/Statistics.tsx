import React from "react";
import { FormControl, Grid, InputLabel, MenuItem, Paper, Select, SelectChangeEvent } from "@mui/material";
import { ArgumentAxis, Chart, SplineSeries, ValueAxis } from "@devexpress/dx-react-chart-material-ui";
import { useInjection } from "inversify-react";
import { StatisticsService } from "../../../core/services/statistics.service";
import { Prices, Statistic, StatsTimeType } from "../../../core/apis/backend/generated";

type PriceTypes = keyof Prices;
type DataType = Record<PriceTypes, number> & {
	date: string;
};

const priceTypes = ["e85", "e10", "gazole", "sp95", "sp98"] as PriceTypes[];

export function Statistics() {
	const services = {
		stats: useInjection(StatisticsService),
	};

	const [timeInterval, setTimeInterval] = React.useState(StatsTimeType.Month);

	const [stats, setStats] = React.useState<Statistic[]>([]);

	const [regions, setRegions] = React.useState<string[]>([]);
	const [fuels, setSelectedFuels] = React.useState<typeof priceTypes>(["e10", "gazole", "sp98"]);
	const [region, setSelectedRegion] = React.useState<string>("None");

	React.useEffect(() => {
		services.stats.getWeeklyStats(timeInterval).then((stats) => {
			setStats(stats);
			const regions = new Set<string>();
			stats.forEach((stat) => {
				Object.keys(stat.data.regions).forEach((region) => {
					regions.add(region);
				});
			});
			const arr = [...regions];
			setRegions(arr);
			setSelectedRegion(arr[0]);
		});
	}, [timeInterval, services.stats]);

	const data = React.useMemo(() => {
		const arr = [] as DataType[];
		const statsSorted = [...stats].sort((s1, s2) => (new Date(s1.time).getTime() < new Date(s2.time).getTime() ? -1 : 1));

		statsSorted.forEach((stat) => {
			if (region !== "None") {
				const newData = {
					date: new Date(stat.time).toLocaleDateString(),
				} as DataType;
				Object.entries(stat.data.regions[region].average).forEach(([fuel, val]) => {
					if (fuels.map((x) => x.toLocaleLowerCase()).includes(fuel.toLocaleLowerCase())) {
						newData[fuel.toLocaleLowerCase()] = val;
					}
				});
				if (!arr.find((stat) => stat.date === newData.date)) {
					arr.push(newData);
				}
			}
		});
		console.log(arr);

		return arr;
	}, [stats, region, fuels]);

	console.log(data);

	const handleFuelChange = (event: SelectChangeEvent<typeof fuels>) => {
		let value = event.target.value;
		setSelectedFuels(value as any);
	};

	const chart = React.useCallback(
		() => (
			<Chart data={data}>
				<ArgumentAxis />
				<ValueAxis />
				{fuels.map((price) => (
					<SplineSeries key={price} name="line" valueField={price} argumentField="date" />
				))}
			</Chart>
		),
		[data, fuels]
	);

	return (
		<Paper sx={{ width: "90%" }}>
			<Grid container spacing={2} display={"flex"}>
				<Grid item>
					<FormControl>
						<InputLabel id="time-interval-label">Fuels</InputLabel>
						<Select multiple labelId="time-fuels-label" id="time-fuels-select" value={fuels} label="Fuels" onChange={handleFuelChange}>
							{priceTypes.map((val) => (
								<MenuItem value={val} key={val}>
									{val}
								</MenuItem>
							))}
						</Select>
					</FormControl>
				</Grid>

				<Grid item>
					<FormControl>
						<InputLabel id="time-interval-label">Time Interval</InputLabel>
						<Select
							labelId="time-interval-label"
							id="time-interval-select"
							value={timeInterval}
							label="Time Interval"
							onChange={(e) => setTimeInterval(e.target.value as StatsTimeType)}
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
					<FormControl>
						<InputLabel id="demo-region-label">Region</InputLabel>
						<Select labelId="demo-region-label" id="demo-region-select" value={region} label="Region" onChange={(e) => setSelectedRegion(e.target.value)}>
							<MenuItem value="None">None</MenuItem>
							{regions.map((region) => (
								<MenuItem key={region} value={region}>
									{region}
								</MenuItem>
							))}
						</Select>
					</FormControl>
				</Grid>
			</Grid>

			<Grid item>{chart()}</Grid>
		</Paper>
	);
}
