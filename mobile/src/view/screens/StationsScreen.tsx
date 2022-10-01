import * as React from "react";
import { Dimensions, StyleSheet, View, VirtualizedList } from "react-native";
import { useAppSelector } from "../../store";
import { Divider, Paragraph, Surface, Title } from "react-native-paper";
import { getFuelStationsNow } from "../../store/stations/stations.action";
import { FuelStationDataDistance, Prices } from "../../core/apis/backend/generated";
import { Services } from "../../core/services";
import { Station } from "../components/fuel/Station";
import { useDispatch } from "react-redux";
import { addDeveloperInput } from "../../store/global/global.action";
import MultiSlider, { MarkerProps } from "@ptomasroos/react-native-multi-slider";

import { theme } from "../constants/Colors";
import { Picker } from "@react-native-picker/picker";
import { fuelsLabels } from "../constants/stations";
import { NoStationFound } from "../components/fuel/NoStationFound";

export function StationsScreen() {
	const coords = useAppSelector((s) => s.location.data?.coords);
	const allData = useAppSelector((s) => s.stations.now);

	const dispatch = useDispatch();

	const [radius, setRadius] = React.useState(2);
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

	const onRangeChange = React.useCallback(([x]: number[]) => setRadius(x), []);

	return (
		<Surface style={styles.container}>
			<View style={{ paddingTop: 30 }}>
				<Title onPress={addDeveloperTouch}>Stations</Title>
				<Divider style={styles.separator} />
			</View>

			<View>
				<View style={styles.innerContainer}>
					<Paragraph style={{ paddingRight: 20 }}>Radius:</Paragraph>
					<MultiSlider step={1} min={1} max={9} onValuesChange={onRangeChange} values={[radius]} customMarker={CustomMaker} />
				</View>
				<View style={styles.innerContainer}>
					<Paragraph style={{ paddingRight: 30 }}>Fuel:</Paragraph>
					<Picker
						enabled
						selectedValue={sortBy}
						dropdownIconColor={"white"}
						style={{
							height: 15,
							width: "38%",
							backgroundColor: theme.colors.surface,
							color: "white",
						}}
						onValueChange={(itemValue) => setSortBy(itemValue)}
					>
						{Object.entries(fuelsLabels).map(([key, val], index) => (
							<Picker.Item
								label={val}
								value={key}
								color={key === sortBy ? theme.colors.primary : undefined}
								style={{
									backgroundColor: theme.colors.surface,
									color: "white",
								}}
							/>
						))}
					</Picker>
				</View>
			</View>

			<Divider style={{ ...styles.separator, marginVertical: 25 }} />

			{data.length > 0 ? (
				<VirtualizedList
					data={data}
					initialNumToRender={4}
					renderItem={({ item }) => <Station key={item.id} item={item} onLongPress={onFuelStationClick(item)} />}
					keyExtractor={(item: FuelStationDataDistance) => item.id.toString()}
					getItemCount={() => data.length}
					getItem={(data, index) => data[index]}
				/>
			) : (
				<NoStationFound fuel={sortBy} radius={radius} />
			)}
		</Surface>
	);
}

function CustomMaker({ currentValue }: MarkerProps) {
	return (
		<View style={{ padding: 8, borderRadius: 100, zIndex: 2, backgroundColor: theme.colors.background }}>
			<Paragraph>{currentValue} km</Paragraph>
		</View>
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
	noStationFoundContainer: { height: "63%" },
	separator: {
		backgroundColor: "lightgray",
		marginVertical: 30,
		height: 1,
		width: width * 0.85,
	},
});
