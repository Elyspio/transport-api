import * as React from "react";
import { StyleSheet, View, VirtualizedList } from "react-native";
import { useAppDispatch, useAppSelector } from "../../store";
import { Caption, Divider, Paragraph, Surface, Title, ToggleButton } from "react-native-paper";
import { getFuelStations } from "../../store/stations/stations.action";
import Slider from "@react-native-community/slider";
import { FuelStationData, Prices } from "../../core/apis/backend/generated";

const fuelTypes: (keyof Prices)[] = ["e10", "gazole"];

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
	}, [coords]);

	return (
		<Surface style={styles.container}>
			<View style={{ paddingTop: 50 }}>
				<Title>Stations</Title>
				<Divider style={styles.separator} />
			</View>

			<View>
				<View style={{ marginBottom: "auto", display: "flex", flexDirection: "row", alignItems: "center" }}>
					<Paragraph style={{ paddingRight: 20 }}>Radius: {radius}</Paragraph>

					<Slider
						style={{ width: 235, height: 50 }}
						step={1}
						minimumValue={1}
						maximumValue={9}
						onValueChange={setRadius}
						value={radius}
						minimumTrackTintColor="#FFFFFF"
						maximumTrackTintColor="#000000"
					/>
				</View>
				<View style={{ marginBottom: "auto", display: "flex", flexDirection: "row", alignItems: "center" }}>
					<Paragraph style={{ width: 95 }}>Sort by </Paragraph>

					<ToggleButton.Row onValueChange={(value: any) => setSortBy(value)} value={sortBy}>
						<ToggleButton icon={() => <Paragraph>e10</Paragraph>} value="e10" style={{ width: 100 }} />
						<ToggleButton icon={() => <Paragraph>gazole</Paragraph>} value="gazole" style={{ width: 100 }} />
					</ToggleButton.Row>
				</View>
			</View>

			{/*<Paragraph>nb pdv : {data.length}</Paragraph>*/}

			<Divider style={{ ...styles.separator, marginBottom: 25, marginTop: 25 }} />

			<VirtualizedList
				data={data}
				initialNumToRender={4}
				renderItem={({ item }) => (
					<View style={{ width: 350, marginTop: 10, marginBottom: 10 }}>
						<Paragraph>{item.location.address}</Paragraph>
						{Object.keys(item.prices)
							.filter((key) => item.prices[key as keyof Prices])
							.map((key) => (
								<Caption>
									{key}: {item.prices[key as keyof Prices]}
								</Caption>
							))}
					</View>
				)}
				keyExtractor={(item: FuelStationData) => item.id.toString()}
				getItemCount={() => data.length}
				getItem={(data, index) => data[index]}
			/>
		</Surface>
	);
}

export default StationsScreen;

const styles = StyleSheet.create({
	container: {
		flex: 1,
		alignItems: "center",
		justifyContent: "center",
	},
	title: {
		fontSize: 20,
		fontWeight: "bold",
	},
	separator: {
		marginVertical: 30,
		height: 1,
		width: "80%",
	},
});
