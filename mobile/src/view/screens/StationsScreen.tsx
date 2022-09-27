import * as React from "react";
import { Dimensions, StyleSheet, View, VirtualizedList } from "react-native";
import { useAppSelector } from "../../store";
import { Divider, Paragraph, Surface, Title, ToggleButton } from "react-native-paper";
import { getFuelStationsNow } from "../../store/stations/stations.action";
import Slider from "@react-native-community/slider";
import { FuelStationDataDistance, Prices } from "../../core/apis/backend/generated";
import { Services } from "../../core/services";
import { Station } from "../components/fuel/Station";
import { PriceSortSelector } from "../components/fuel/PriceSortSelector";
import { useDispatch } from "react-redux";
import { addDeveloperInput } from "../../store/global/global.action";

export function StationsScreen() {
	const coords = useAppSelector((s) => s.location.data?.coords);
	const allData = useAppSelector((s) => s.stations.now);

	const dispatch = useDispatch();

	const [radius, setRadius] = React.useState(1);
	const [sortBy, setSortBy] = React.useState<keyof Prices>("e10");

	const data = React.useMemo(() => {
		let ret = [...allData].filter((a) => a.prices[sortBy]?.length > 0);
		ret.sort((stationA, stationB) => {
			return stationA.prices[sortBy][0].value < stationB.prices[sortBy][0].value ? -1 : 1;
		});
		return ret;
	}, [sortBy, allData]);

	const addDeveloperTouch = React.useCallback(() => dispatch(addDeveloperInput()), [dispatch]);

	React.useEffect(() => {
		if (coords) {
			dispatch(
				getFuelStationsNow({
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
				<Title onPress={addDeveloperTouch}>Stations</Title>
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

					<ToggleButton.Row onValueChange={(value: any) => value && setSortBy(value)} value={sortBy}>
						<PriceSortSelector selected={sortBy === "e10"} value={"e10"} />
						<PriceSortSelector selected={sortBy === "gazole"} value={"gazole"} />
					</ToggleButton.Row>
				</View>
			</View>

			<Divider style={{ ...styles.separator, marginVertical: 25 }} />

			<VirtualizedList
				data={data}
				initialNumToRender={4}
				renderItem={({ item }) => <Station key={item.id} item={item} onLongPress={onFuelStationClick(item)} />}
				keyExtractor={(item: FuelStationDataDistance) => item.id.toString()}
				getItemCount={() => data.length}
				getItem={(data, index) => data[index]}
			/>
		</Surface>
	);
}

export default StationsScreen;

const { width } = Dimensions.get("window");

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
});
