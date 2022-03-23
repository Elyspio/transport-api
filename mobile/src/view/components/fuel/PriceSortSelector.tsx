import React from "react";
import { Paragraph, ToggleButton, useTheme } from "react-native-paper";
import { Prices } from "../../../core/apis/backend/generated";

export type PriceValues = keyof Prices;
type PriceSortSelectorProps = { selected: boolean; value: PriceValues };

export function PriceSortSelector({ selected, value }: PriceSortSelectorProps) {
	const theme = useTheme();

	return <ToggleButton icon={() => <Paragraph>{value}</Paragraph>} value={value} style={{ width: 100, backgroundColor: selected ? theme.colors.primary : undefined }} />;
}
