import * as React from "react";
import { StyleSheet } from "react-native";
import { useAppSelector } from "../../store";
import { Divider, Paragraph, Surface } from "react-native-paper";

export function StoreScreen() {
	const store = useAppSelector((s) => s);

	return (
		<Surface style={styles.container}>
			<Paragraph style={styles.title}>Store</Paragraph>
			<Divider style={{ marginBottom: 30 }} />
			<Paragraph> {JSON.stringify(store, null, 4)}</Paragraph>
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
