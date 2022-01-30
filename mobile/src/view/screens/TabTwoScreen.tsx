import * as React from "react";
import { StyleSheet } from "react-native";

import { Divider, Surface, Text } from "react-native-paper";

export default function TabTwoScreen() {
	return (
		<Surface style={styles.container}>
			<Text style={styles.title}>Tab Two</Text>
			<Divider />
		</Surface>
	);
}

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
