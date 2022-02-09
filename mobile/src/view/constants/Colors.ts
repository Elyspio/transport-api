import { DarkTheme } from "react-native-paper";
import type { Theme } from "react-native-paper/src/types";
import { Appearance } from "react-native";

export const theme: Theme = {
	...DarkTheme,
	roundness: 2,
	dark: Appearance.getColorScheme() === "dark",
	colors: {
		...DarkTheme.colors,
		primary: "#3498db",
		accent: "#f1c40f",
		background: "#212121",
	},
};
