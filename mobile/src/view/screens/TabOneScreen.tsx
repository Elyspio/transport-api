import * as React from "react";
import { StyleSheet } from "react-native";

import { useAppDispatch, useAppSelector } from "../../store";
import { add } from "../../store/example";
import { Divider, Surface, Text, Title } from "react-native-paper";

export function TabOneScreen() {
	const val = useAppSelector((s) => s.example.value);

	const dispatch = useAppDispatch();

	React.useEffect(() => {
		setInterval(() => {
			dispatch(add(Math.floor(Math.random() * 100)));
		}, 100);
	}, [dispatch]);

	return (
		<Surface style={styles.container}>
			<Title>Tab One</Title>
			<Divider />
			<Text>{val}</Text>
		</Surface>
	);
}

export default TabOneScreen;

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
