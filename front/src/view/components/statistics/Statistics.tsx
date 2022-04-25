import React from "react";
import { Box, Divider, Grid, Paper, Typography } from "@mui/material";
import { StatControls } from "./controls/StatControls";
import { useAppDispatch, useAppSelector } from "../../../store";
import { getStatistics } from "../../../store/module/statistics/statistics.action";
import { Chart } from "./chart/Chart";

export function Statistics() {
	const {
		selected: { timeInterval },
	} = useAppSelector((s) => s.statistic);

	const dispatch = useAppDispatch();

	React.useEffect(() => {
		dispatch(getStatistics(timeInterval));
	}, [timeInterval, dispatch]);

	return (
		<Paper sx={{ width: "90%" }}>
			<Grid container alignItems={"center"} justifyContent={"center"} mb={4}>
				<Grid item p={2}>
					<Typography variant={"overline"} fontSize={"150%"}>
						Évolution des prix
					</Typography>
				</Grid>
				<Grid item container px={4}>
					<Divider sx={{ border: 0.5, width: "100%", opacity: 0.2 }}></Divider>
				</Grid>
			</Grid>

			<Grid container width={"100%"}>
				<Grid item xs={3}>
					<StatControls />
				</Grid>

				<Grid item xs={9} pr={3}>
					<Box p={2} width={"100%"} height={"100%"}>
						<Chart />
					</Box>
				</Grid>
			</Grid>
		</Paper>
	);
}
