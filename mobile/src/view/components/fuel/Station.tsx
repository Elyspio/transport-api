import { FuelStationDataDistance } from "../../../core/apis/backend/generated";
import { Dimensions, StyleSheet, TouchableHighlight, View } from "react-native";
import { Caption, Paragraph } from "react-native-paper";
import { Price } from "./Price";
import * as React from "react";
import { PriceValues } from "./PriceSortSelector";
import { theme } from "../../constants/Colors";

function getDistance(val: number) {
	if (val < 1000) {
		return `(${val.toFixed(0)}m)`;
	} else {
		return `(${(val / 1000).toFixed(2)}Km)`;
	}
}

export function Station(props: { item: FuelStationDataDistance; onLongPress: () => void }) {
	let { item, onLongPress } = props;

	const prices = React.useMemo(() => {
		return Object.keys(item.prices)
			.filter((fuel) => item.prices[fuel as PriceValues].length)
			.map((fuel) => <Price key={fuel} value={item.prices[fuel as PriceValues][0].value!} fuel={fuel as PriceValues} />);
	}, [item]);

	return (
		<TouchableHighlight onLongPress={onLongPress}>
			<View style={styles.item}>
				<Paragraph>{item.location.address}</Paragraph>
				<Caption>City: {item.location.city}</Caption>
				<Caption>Distance: {getDistance(item.distance)}</Caption>
				{prices}
			</View>
		</TouchableHighlight>
	);
}

const { width } = Dimensions.get("window");

const styles = StyleSheet.create({
	item: {
		width: width * 0.9,
		marginVertical: 5,
		backgroundColor: theme.colors.background,
		padding: 10,
		paddingHorizontal: 20,
	},
});
