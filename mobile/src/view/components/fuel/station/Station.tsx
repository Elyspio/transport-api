import { FuelStationDataDistance } from "../../../../core/apis/backend/generated";
import { Dimensions, StyleSheet, TouchableHighlight, ViewStyle } from "react-native";
import { Caption, Paragraph } from "react-native-paper";
import { Price } from "../Price";
import * as React from "react";
import { PriceValues } from "../../../constants/stations";
import { getDistance, StationModal } from "./StationModal";
import { Box } from "native-base";

const { width } = Dimensions.get("screen");

export function Station(props: { data: FuelStationDataDistance }) {
	let { data } = props;

	const [modalVisible, setModalVisible] = React.useState(false);

	const prices = React.useMemo(() => {
		return Object.keys(data.prices)
			.filter((fuel) => data.prices[fuel as PriceValues].length)
			.map((fuel) => {
				const { date, value } = data.prices[fuel as PriceValues][0];
				return <Price key={fuel} value={value!} fuel={fuel as PriceValues} date={date} />;
			});
	}, [data]);

	return (
		<Box>
			<TouchableHighlight onPress={() => setModalVisible(true)}>
				<Box style={styles.item}>
					<Paragraph>{data.location.address}</Paragraph>
					<Caption>City: {data.location.city}</Caption>
					<Caption>Distance: {getDistance(data.distance)}</Caption>
					{prices}
				</Box>
			</TouchableHighlight>

			<StationModal modal={{ get: modalVisible, set: setModalVisible }} data={data} />
		</Box>
	);
}

const common: ViewStyle = {
	width: width * 0.9,
	marginVertical: 5,
	padding: 10,
	paddingHorizontal: 20,
};

const styles = StyleSheet.create({
	item: common,
	centeredView: {
		height: "100%",
		width: "100%",
		display: "flex",
		alignItems: "center",
		justifyContent: "center",
	},
});
