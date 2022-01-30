import * as React from "react";
import { StyleSheet } from "react-native";

import { useAppDispatch, useAppSelector } from "../../store";
import { add } from "../../store/example";
import { Divider, Surface, Text, Title } from "react-native-paper";
import { fuelStationApi } from "../../core/apis";
import { FuelStationData } from "../../core/apis/backend/generated";

export function StationsScreen() {
	const val = useAppSelector((s) => s.example.value);

	const dispatch = useAppDispatch();

	React.useEffect(() => {
		const interval = setInterval(() => {
			dispatch(add(Math.floor(Math.random() * 100)));
		}, 500);

		return () => clearInterval(interval);
	}, [dispatch]);

	const [data, setData] = React.useState<FuelStationData[]>([]);

	React.useEffect(() => {
		fuelStationApi.getFuelStations(45.793611709275226, 4.8451552630342).then(({ data }) => {
			setData(data);
		});
	}, []);

	return (
		<Surface style={styles.container}>
			<Title>Stations</Title>
			<Text>{val}</Text>
			<Divider style={styles.separator} />
			<Text>nb pdv : {data.length}</Text>
			{data.map((pdv) => (
				<Text key={pdv.id}>{pdv.location.address}</Text>
			))}
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
