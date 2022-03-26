import * as React from "react";
import { Dimensions, ScrollView, StyleSheet } from "react-native";
import { useAppSelector } from "../../store";
import { Divider, Paragraph, Surface, Text } from "react-native-paper";

export function StoreScreen() {
	const store = useAppSelector((s) => s);

	return (
		<Surface style={styles.container}>
			<Paragraph style={styles.title}>Store</Paragraph>
			<Divider style={{ marginBottom: 30 }} />
			<ScrollView>
				<Text>{JSON.stringify(store, null, 8)})</Text>
			</ScrollView>
		</Surface>
	);
}

const { height } = Dimensions.get("window");

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
	content: {
		maxHeight: height - 200,
		overflow: "scroll",
	},
});
