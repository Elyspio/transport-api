import React from "react";
import { Paragraph, Surface, useTheme } from "react-native-paper";
import { StyleSheet } from "react-native";
import { fuelsLabels } from "../../constants/stations";
import { PriceValues } from "./PriceSortSelector";

const styles = StyleSheet.create({
	innerContainer: { marginBottom: "auto", display: "flex", flexDirection: "row", alignItems: "center" },
	noStationFoundContainer: { height: "65%" },
});

type NoStationFoundProps = { fuel: PriceValues; radius: number };

export function NoStationFound({ fuel, radius }: NoStationFoundProps) {
	const theme = useTheme();

	return (
		<Surface style={styles.noStationFoundContainer}>
			<Paragraph>
				No station offering {fuelsLabels[fuel]} was found within {radius} km
			</Paragraph>
		</Surface>
	);
}
