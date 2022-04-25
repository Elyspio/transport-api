import React from "react";
import { useTheme } from "@mui/material";

export function getShortDate(date: Date) {
	const month = date.getMonth() + 1;
	const day = date.getDate();
	const year = date.getFullYear();
	return `${day}/${month}/${year}`;
}

export function CustomizedAxisTick(props: any) {
	const { x, y, payload } = props;

	const { palette } = useTheme();

	const color = palette.text.primary;

	return (
		<g transform={`translate(${x},${y})`}>
			<text x={25} y={5} dy={16} textAnchor="end" fill={`${color}`} transform="rotate(-20)" opacity={0.4}>
				{getShortDate(new Date(payload.value))}
			</text>
		</g>
	);
}
