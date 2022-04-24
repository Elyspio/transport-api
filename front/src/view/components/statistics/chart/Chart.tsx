import React from "react";
import { Legend, Line, LineChart, ResponsiveContainer, Tooltip, XAxis, YAxis } from "recharts";
import { useAppSelector } from "../../../../store";
import { ChartTooltip } from "./ChartTooltip";
import { CustomizedAxisTick } from "./CustomizedAxisTick";
import { chartColors } from "../../../../config/colors.chart";
import { Box } from "@mui/material";

export function Chart() {
	const {
		data,
		selected: { fuels, switches },
	} = useAppSelector((s) => s.statistic);

	return (
		<Box sx={{ width: "100%", height: "100%" }}>
			<ResponsiveContainer width={"100%"} aspect={2}>
				<LineChart
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
					<YAxis type={"number"} domain={switches.yAxisFrom0 ? undefined : ["auto", "auto"]} />
					{fuels.map((fuel) => (
						<Line key={fuel} type="monotone" dataKey={fuel} stroke={chartColors[fuel]} />
					))}
				</LineChart>
			</ResponsiveContainer>
		</Box>
	);
}
