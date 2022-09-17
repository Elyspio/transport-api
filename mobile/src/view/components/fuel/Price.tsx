import { Caption } from "react-native-paper";
import * as React from "react";
import { Prices } from "../../../core/apis/backend/generated";
import { useAppSelector } from "../../../store";

type PriceProps = {
	value: number;
	fuel: keyof Prices;
};

export function Price({ value, fuel }: PriceProps) {
	const lower = useAppSelector((s) => s.stations.lowest);

	const delta = React.useMemo(() => (lower ? value - (lower[fuel] ?? 0) : null), [lower]);

	const color = React.useMemo(() => {
		if (delta === null) return;
		if (delta === 0) return "#0F0";
		if (delta < 0.05) return "#BF0";
		if (delta < 0.1) return "#FF0";
		if (delta < 0.15) return "#FB0";
		if (delta < 0.2) return "#F90";
		return "#F00";
	}, [delta]);

	console.log({ delta, lower });

	return (
		<Caption style={{ color: color, opacity: 0.8 }}>
			{fuel.toLocaleUpperCase()}: {value} € {delta !== 0 && `(+${delta?.toFixed(3)})`}
		</Caption>
	);
}
