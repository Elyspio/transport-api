import React from "react";
import { useTheme } from "@mui/material";

export function CustomizedAxisTick(props: any) {
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
}
