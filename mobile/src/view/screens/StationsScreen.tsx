import * as React from "react";
import { Dimensions, StyleSheet, TouchableHighlight, View, VirtualizedList } from "react-native";
import { useAppDispatch, useAppSelector } from "../../store";
import { Caption, Divider, Paragraph, Surface, Title, ToggleButton } from "react-native-paper";
import { getFuelStations } from "../../store/stations/stations.action";
import Slider from "@react-native-community/slider";
import { FuelStationDataDistance, Prices } from "../../core/apis/backend/generated";
import { theme } from "../constants/Colors";
import { Services } from "../../core/services";

const fuelTypes: (keyof Prices)[] = ["e10", "gazole"];

// https://www.waze.com/fr/live-map/directions?to=ll.45.78127997%2C4.82977867&from=ll.45.7935651%2C4.8451191

function getDistance(val: number) {
	if (val < 1000) {
		return `(${val.toFixed(0)}m)`;
	} else {
		return `(${(val / 1000).toFixed(2)}Km)`;
	}
}

export function StationsScreen() {
	const coords = useAppSelector((s) => s.location.data?.coords);
	const allData = useAppSelector((s) => s.stations.data);

	const dispatch = useAppDispatch();

	const [radius, setRadius] = React.useState(1);
	const [sortBy, setSortBy] = React.useState<keyof Prices>("e10");

	const data = React.useMemo(() => {
		let ret = [...allData];
		if (sortBy) ret = ret.filter((a) => a.prices[sortBy]);
		ret.sort((a, b) => (a.prices[sortBy]! > b.prices[sortBy]! ? 1 : -1));
		return ret;
	}, [sortBy, allData]);

	React.useEffect(() => {
		if (coords) {
			dispatch(
				getFuelStations({
					latitude: coords.latitude,
					longitude: coords.longitude,
					radius,
				})
			);
		}
	}, [dispatch, coords, radius]);

	const onFuelStationClick = React.useCallback(
		(station: FuelStationDataDistance) => () => {
			return Services.waze.openWaze({
				longitude: station.location.longitude,
				latitude: station.location.latitude,
			});
		},
		[]
	);

	return (
		<Surface style={styles.container}>
			<View style={{ paddingTop: 30 }}>
				<Title>Stations</Title>
				<Divider style={styles.separator} />
			</View>

			<View>
				<View style={styles.innerContainer}>
					<Paragraph style={{ paddingRight: 20 }}>Radius: {radius}</Paragraph>

					<Slider
						style={{ width: width * 0.6, height: 50 }}
						step={1}
						minimumValue={1}
						maximumValue={9}
						onValueChange={setRadius}
						value={radius}
						minimumTrackTintColor="#FFFFFF"
						maximumTrackTintColor="#000000"
					/>
				</View>
				<View style={styles.innerContainer}>
					<Paragraph style={{ width: 95 }}>Sort by </Paragraph>

					<ToggleButton.Row onValueChange={(value: any) => setSortBy(value)} value={sortBy}>
						<ToggleButton icon={() => <Paragraph>e10</Paragraph>} value="e10" style={{ width: 100 }} />
						<ToggleButton icon={() => <Paragraph>gazole</Paragraph>} value="gazole" style={{ width: 100 }} />
					</ToggleButton.Row>
				</View>
			</View>

			<Divider style={{ ...styles.separator, marginVertical: 25 }} />

			<VirtualizedList
				data={data}
				initialNumToRender={4}
				renderItem={({ item }) => (
					<TouchableHighlight onLongPress={onFuelStationClick(item)}>
						<View style={styles.item}>
							<Paragraph>
								{item.location.address} {getDistance(item.distance)}
							</Paragraph>
							<Caption>City: {item.location.city}</Caption>
							<Caption>Distance: {item.location.city}</Caption>
							{item.prices.gazole && <Caption>Gazole: {item.prices.gazole} €</Caption>}
							{item.prices.e10 && <Caption>E10: {item.prices.e10} €</Caption>}
							{item.prices.sp98 && <Caption>SP98: {item.prices.sp98} €</Caption>}
							{item.prices.sp95 && <Caption>SP95: {item.prices.sp95} €</Caption>}
							{item.prices.e85 && <Caption>E85: {item.prices.e85} €</Caption>}
							{item.prices.gpLc && <Caption>GPL: {item.prices.gpLc} €</Caption>}
						</View>
					</TouchableHighlight>
				)}
				keyExtractor={(item: FuelStationDataDistance) => item.id.toString()}
				getItemCount={() => data.length}
				getItem={(data, index) => data[index]}
			/>
		</Surface>
	);
}

export default StationsScreen;

const { width, height } = Dimensions.get("window");

const styles = StyleSheet.create({
	container: {
		flex: 1,
		alignItems: "center",
		justifyContent: "center",
	},
	innerContainer: { marginBottom: "auto", display: "flex", flexDirection: "row", alignItems: "center" },
	title: {
		fontSize: 20,
		fontWeight: "bold",
	},
	separator: {
		marginVertical: 30,
		height: 1,
		width: width * 0.8,
	},
	item: {
		width: width * 0.9,
		marginVertical: 5,
		backgroundColor: theme.colors.background,
		padding: 10,
		paddingHorizontal: 20,
	},
});
