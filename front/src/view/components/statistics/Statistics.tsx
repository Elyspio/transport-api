import React from "react";
import { Box, Grid, Paper } from "@mui/material";
import StatControls from "./StatControls";
import { useAppDispatch, useAppSelector } from "../../../store";
import { getStatistics } from "../../../store/module/statistics/statistics.action";
import { Chart } from "./chart/Chart";

export function Statistics() {
	const {
		data,
		selected: { fuels, timeInterval },
	} = useAppSelector((s) => s.statistic);

	const dispatch = useAppDispatch();

	React.useEffect(() => {
		dispatch(getStatistics(timeInterval));
	}, [timeInterval]);

	return (
		<Paper sx={{ width: "90%" }}>
			<Grid container width={"100%"}>
				<Grid item xs={3}>
					<StatControls />
				</Grid>

				<Grid item xs={9}>
					<Box p={2} width={"100%"} height={"100%"}>
						<Chart />
					</Box>
				</Grid>
			</Grid>
		</Paper>
	);
}
