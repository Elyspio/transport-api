import { StatusBar } from "expo-status-bar";
import React from "react";
import { SafeAreaProvider } from "react-native-safe-area-context";
import { useCachedResources } from "./src/view/hooks/useCachedResources";
import Navigation from "./src/view/navigation";
import { Provider } from "react-redux";
import { store } from "./src/store";
import { Provider as PaperProvider } from "react-native-paper";
import { nativeBaseTheme, theme } from "./src/view/constants/Colors";
import { useColorScheme } from "react-native";
import "react-native-url-polyfill/auto";
import { NativeBaseProvider } from "native-base";

export default function App() {
	const isLoadingComplete = useCachedResources();
	const colorScheme = useColorScheme();

	if (!isLoadingComplete) {
		return null;
	} else {
		return (
			<Provider store={store}>
				<SafeAreaProvider>
					<NativeBaseProvider theme={nativeBaseTheme}>
						<PaperProvider theme={theme}>
							<Navigation colorScheme={colorScheme} />
							<StatusBar hidden={true} />
						</PaperProvider>
					</NativeBaseProvider>
				</SafeAreaProvider>
			</Provider>
		);
	}
}
