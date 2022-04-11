import React, { useMemo, useRef } from "react";
import { Legend, Line, LineChart, Tooltip, XAxis, YAxis } from "recharts";
import { useAppSelector } from "../../../../store";
import { ChartTooltip } from "./ChartTooltip";
import { CustomizedAxisTick } from "./CustomizedAxisTick";
import { chartColors } from "../../../../config/colors.chart";
import { Box } from "@mui/material";

export function Chart() {
	const {
		data,
		selected: { fuels, timeInterval },
	} = useAppSelector((s) => s.statistic);

	const ref = useRef<HTMLElement>(null);

	const { width, height } = useMemo(() => {
		const width = ref.current?.clientWidth ?? 0;
		const height = (ref.current?.clientHeight ?? 0) + 200;
		return {
			width,
			height,
		};
	}, [ref.current]);

	console.log({ width, height });

	return (
		<Box ref={ref} sx={{ width: "100%", height: "100%" }}>
			<LineChart
				width={width}
				height={height}
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
					<Line key={fuel} type="monotone" dataKey={fuel} stroke={chartColors[fuel]} />
				))}
			</LineChart>
		</Box>
	);
}
