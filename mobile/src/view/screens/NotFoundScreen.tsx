import { StackScreenProps } from "@react-navigation/stack";
import * as React from "react";
import { StyleSheet, TouchableOpacity } from "react-native";
import { Surface, Text } from "react-native-paper";
import { RootStackParamList } from "../../core/types/navigation";

export default function NotFoundScreen({ navigation }: StackScreenProps<RootStackParamList, "NotFound">) {
	return (
		<Surface style={styles.container}>
			<Text style={styles.title}>This screen doesn't exist.</Text>
			<TouchableOpacity onPress={() => navigation.replace("Root")} style={styles.link}>
				<Text style={styles.linkText}>Go to home screen!</Text>
			</TouchableOpacity>
		</Surface>
	);
}

const styles = StyleSheet.create({
	container: {
		flex: 1,
		backgroundColor: "#fff",
		alignItems: "center",
		justifyContent: "center",
		padding: 20,
	},
	title: {
		fontSize: 20,
		fontWeight: "bold",
	},
	link: {
		marginTop: 15,
		paddingVertical: 15,
	},
	linkText: {
		fontSize: 14,
		color: "#2e78b7",
	},
});
