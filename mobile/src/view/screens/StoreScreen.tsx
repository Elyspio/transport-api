import * as React from "react";
import { StyleSheet } from "react-native";
import { useAppSelector } from "../../store";
import { Divider, Surface, Text } from "react-native-paper";

export function StoreScreen() {
	const store = useAppSelector((s) => s);

	return (
		<Surface style={styles.container}>
			<Text style={styles.title}>Store</Text>
			<Divider style={{ marginBottom: 30 }} />
			<Text> {JSON.stringify(store, null, 4)}</Text>
		</Surface>
	);
}

export default StoreScreen;
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
