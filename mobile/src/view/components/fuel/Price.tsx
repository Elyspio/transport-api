import { Caption } from "react-native-paper";
import * as React from "react";
import { Prices } from "../../../core/apis/backend/generated";

type PriceProps = {
	value: number;
	fuel: keyof Prices;
};

export function Price({ value, fuel }: PriceProps) {
	return (
		<Caption>
			{fuel.toLocaleUpperCase()}: {value} €
		</Caption>
	);
}
