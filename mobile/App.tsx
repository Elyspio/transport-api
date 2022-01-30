import { StatusBar } from "expo-status-bar";
import React from "react";
import { SafeAreaProvider } from "react-native-safe-area-context";
import { useCachedResources } from "./src/view/hooks/useCachedResources";
import { useColorScheme } from "./src/view/hooks/useColorScheme";
import Navigation from "./src/view/navigation";
import { Provider } from "react-redux";
import { store } from "./src/store";
import { Provider as PaperProvider } from "react-native-paper";
import { theme } from "./src/view/constants/Colors";

export default function App() {
	const isLoadingComplete = useCachedResources();
	const colorScheme = useColorScheme();

	if (!isLoadingComplete) {
		return null;
	} else {
		return (
			<Provider store={store}>
				<SafeAreaProvider>
					<PaperProvider theme={theme}>
						<Navigation colorScheme={colorScheme} />
						<StatusBar hidden={true} />
					</PaperProvider>
				</SafeAreaProvider>
			</Provider>
		);
	}
}
