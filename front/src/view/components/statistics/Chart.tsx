import React, { FunctionComponent } from "react";
import { Legend, Line, LineChart, Tooltip, TooltipProps, XAxis, YAxis } from "recharts";
import { PriceTypes } from "../../../store/module/statistics/statistics.action";
import { useAppSelector } from "../../../store";
import { Divider, Grid, Paper, Typography, useTheme } from "@mui/material";

const data = [
	{
		name: "Page A",
		uv: 4000,
		pv: 2400,
		amt: 2400,
	},
	{
		name: "Page B",
		uv: 3000,
		pv: 1398,
		amt: 2210,
	},
	{
		name: "Page C",
		uv: 2000,
		pv: 9800,
		amt: 2290,
	},
	{
		name: "Page D",
		uv: 2780,
		pv: 3908,
		amt: 2000,
	},
	{
		name: "Page E",
		uv: 1890,
		pv: 4800,
		amt: 2181,
	},
	{
		name: "Page F",
		uv: 2390,
		pv: 3800,
		amt: 2500,
	},
	{
		name: "Page G",
		uv: 3490,
		pv: 4300,
		amt: 2100,
	},
];

const CustomizedLabel: FunctionComponent<any> = (props: any) => {
	const { x, y, stroke, value } = props;

	return (
		<text x={x} y={y} dy={-4} fill={stroke} fontSize={10} textAnchor="middle">
			{value}
		</text>
	);
};

const CustomizedAxisTick: FunctionComponent<any> = (props: any) => {
	const { x, y, payload } = props;

	const { palette } = useTheme();

	const color = palette.text.primary;

	return (
		<g transform={`translate(${x},${y})`}>
			<text x={0} y={0} dy={16} textAnchor="end" fill={`${color}`} transform="rotate(-35)">
				{payload.value}
			</text>
		</g>
	);
};

const colors: Record<PriceTypes, string> = {
	gazole: "#FFAA00",
	e10: "#00ff99",
	sp98: "#0051ff",
	e85: "#ff00dd",
	sp95: "#aaff00",
	gpLc: "#ff0000",
};

function ChartTooltip({ payload, label, active }: TooltipProps<any, any>) {
	if (!active) return null;
	const data = payload?.[0].payload;
	if (!data) return null;
	const fuels = Object.keys(data).filter((key) => key !== "date");

	console.log({ payload, label });
	return (
		<Paper>
			<Grid container p={2} direction={"column"} spacing={1}>
				<Grid item>
					<Typography textAlign={"center"}>{label}</Typography>
				</Grid>

				<Grid item my={1}>
					<Divider></Divider>
				</Grid>

				{fuels.map((fuel) => (
					<Grid container sx={{ color: colors[fuel] }} spacing={1}>
						<Grid item>
							<Typography>{fuel}: </Typography>
						</Grid>
						<Grid item ml={"auto"}>
							<Typography sx={{ color: colors[fuel], textAlign: "right" }}>{data[fuel].toFixed(2)}€</Typography>
						</Grid>
					</Grid>
				))}
			</Grid>
		</Paper>
	);
}

export function Chart() {
	const {
		data,
		selected: { fuels, timeInterval },
	} = useAppSelector((s) => s.statistic);

	return (
		<LineChart
			width={window.innerWidth * 0.5}
			height={window.innerHeight * 0.5}
			data={data}
			margin={{
				top: 20,
				right: 30,
				left: 20,
				bottom: 10,
			}}
		>
			<Tooltip content={<ChartTooltip />} />
			<Legend />
			<XAxis dataKey="date" height={60} tick={<CustomizedAxisTick />} />
			<YAxis type={"number"} domain={["auto", "auto"]} />

			{fuels.map((fuel) => (
				<Line type="monotone" dataKey={fuel} stroke={colors[fuel]} />
			))}
		</LineChart>
	);
}
