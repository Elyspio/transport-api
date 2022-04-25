import { TooltipProps } from "recharts";
import { Divider, Grid, Paper, Typography } from "@mui/material";
import React from "react";
import { chartColors } from "../../../../config/colors.chart";
import { fuelsLabels } from "../controls/StatControls.constants";
import { getShortDate } from "./CustomizedAxisTick";

export function ChartTooltip({ payload, label, active }: TooltipProps<any, any>) {
	if (!active) return null;
	const data = payload?.[0].payload;
	if (!data) return null;
	const fuels = Object.keys(data).filter((key) => key !== "date");

	return (
		<Paper>
			<Grid container p={2} direction={"column"} spacing={1}>
				<Grid item>
					<Typography textAlign={"center"}>{getShortDate(new Date(label))}</Typography>
				</Grid>

				<Grid item my={1}>
					<Divider></Divider>
				</Grid>

				{fuels.map((fuel) => (
					<Grid key={fuel} container sx={{ color: chartColors[fuel] }} spacing={1}>
						<Grid item>
							<Typography>{fuelsLabels[fuel]}: </Typography>
						</Grid>
						<Grid item ml={"auto"}>
							<Typography
								sx={{
									color: chartColors[fuel],
									textAlign: "right",
								}}
							>
								{data[fuel].toFixed(2)}€
							</Typography>
						</Grid>
					</Grid>
				))}
			</Grid>
		</Paper>
	);
}
