import React from "react";
import { Legend, Line, LineChart, Tooltip, XAxis, YAxis } from "recharts";
import { useAppSelector } from "../../../../store";
import { ChartTooltip } from "./ChartTooltip";
import { CustomizedAxisTick } from "./CustomizedAxisTick";
import { chartColors } from "../../../../config/colors.chart";

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
			<Legend
				wrapperStyle={{
					top: 0,
					right: 0,
				}}
			/>
			<XAxis dataKey="date" height={60} tick={<CustomizedAxisTick />} />
			<YAxis type={"number"} domain={["auto", "auto"]} />
			{fuels.map((fuel) => (
				<Line type="monotone" dataKey={fuel} stroke={chartColors[fuel]} />
			))}
		</LineChart>
	);
}
