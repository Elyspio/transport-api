import * as React from "react";
import { Dimensions, StyleSheet, View, VirtualizedList } from "react-native";
import { useAppSelector } from "../../store";
import { Paragraph, Surface, Title } from "react-native-paper";
import { getFuelStationsNow } from "../../store/stations/stations.action";
import { FuelStationDataDistance } from "../../core/apis/backend/generated";
import { Station } from "../components/fuel/station/Station";
import { useDispatch } from "react-redux";
import { addDeveloperInput } from "../../store/global/global.action";
import MultiSlider, { MarkerProps } from "@ptomasroos/react-native-multi-slider";

import { theme } from "../constants/Colors";
import { fuelsLabels, PriceValues } from "../constants/stations";
import { NoStationFound } from "../components/fuel/NoStationFound";
import { CheckIcon, Divider, Select, Stack } from "native-base";

export function StationsScreen() {
	const coords = useAppSelector((s) => s.location.data?.coords);
	const allData = useAppSelector((s) => s.stations.now);

	const dispatch = useDispatch();

	const [radius, setRadius] = React.useState(2);
	const [sortBy, setSortBy] = React.useState<PriceValues>("e10");

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

					<Select
						minW={100}
						_selectedItem={{ endIcon: <CheckIcon size="5" /> }}
						selectedValue={sortBy}
						accessibilityLabel="Choose Fuel"
						onValueChange={(itemValue) => setSortBy(itemValue as PriceValues)}
					>
						{Object.entries(fuelsLabels).map(([key, val], index) => (
							<Select.Item key={val} label={val} value={key} />
						))}
					</Select>
				</View>
			</View>

			<Divider style={{ ...styles.separator, marginVertical: 25 }} />

			{data.length > 0 ? (
				<VirtualizedList
					data={data}
					initialNumToRender={4}
					renderItem={({ item, index, separators }) => (
						<Stack alignItems={"center"}>
							<Station key={item.id} data={item} />
							{index < data.length - 1 ? <Divider width={"80%"} my={3} /> : null}
						</Stack>
					)}
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
