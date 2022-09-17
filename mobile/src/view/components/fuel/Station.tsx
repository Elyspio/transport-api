import { FuelStationDataDistance } from "../../../core/apis/backend/generated";
import { Dimensions, StyleSheet, TouchableHighlight, View, ViewStyle } from "react-native";
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

	const [pressInDate, setPressInDate] = React.useState<Date>();
	const [pressOutDate, setPressOutDate] = React.useState<Date>();
	const [pressed, setPressed] = React.useState(false);

	const prices = React.useMemo(() => {
		return Object.keys(item.prices)
			.filter((fuel) => item.prices[fuel as PriceValues].length)
			.map((fuel) => <Price key={fuel} value={item.prices[fuel as PriceValues][0].value!} fuel={fuel as PriceValues} />);
	}, [item]);

	const onPressIn = React.useCallback(() => {
		setPressInDate(new Date());
		setPressOutDate(undefined);
	}, []);

	const onPressOut = React.useCallback(() => {
		setPressOutDate(new Date());
	}, []);

	React.useEffect(() => {
		if (pressInDate && pressOutDate) {
			let delta = pressOutDate.getTime() - pressInDate.getTime();
			console.log({ delta });
			if (delta > 1000) {
				setPressInDate(undefined);
				setPressOutDate(undefined);
				onLongPress();
			}
		}
	}, [pressOutDate, pressInDate]);

	return (
		<TouchableHighlight onPressIn={onPressIn} onPressOut={onPressOut}>
			<View style={pressed ? styles.pressed : styles.item}>
				<Paragraph>{item.location.address}</Paragraph>
				<Caption>City: {item.location.city}</Caption>
				<Caption>Distance: {getDistance(item.distance)}</Caption>
				{prices}
			</View>
		</TouchableHighlight>
	);
}

const { width } = Dimensions.get("window");

const common: ViewStyle = {
	width: width * 0.9,
	marginVertical: 5,
	backgroundColor: theme.colors.background,
	padding: 10,
	paddingHorizontal: 20,
};

const styles = StyleSheet.create({
	item: common,
	pressed: {
		...common,
		backgroundColor: theme.colors.background,
		border: "1px solid",
	},
});
