import "reflect-metadata";
import React from "react";
import { createRoot } from "react-dom/client";

import "./index.scss";
import { Provider } from "react-redux";
import store, { useAppSelector } from "./store";
import Application from "./view/components/Application";
import { StyledEngineProvider, Theme, ThemeProvider } from "@mui/material";
import { themes } from "./config/theme";
import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.min.css";
import { Provider as DiProvider } from "inversify-react";
import { container } from "./core/di";
import { BrowserRouter } from "react-router-dom";

declare module "@mui/styles/defaultTheme" {
	// eslint-disable-next-line @typescript-eslint/no-empty-interface
	interface DefaultTheme extends Theme {}
}

function Wrapper() {
	const { theme, current } = useAppSelector((state) => ({
		theme: state.theme.current === "dark" ? themes.dark : themes.light,
		current: state.theme.current,
	}));

	const basename = process.env.NODE_ENV === "production" ? "/transport" : undefined;

	return (
		<StyledEngineProvider injectFirst>
			<ThemeProvider theme={theme}>
				<BrowserRouter basename={basename}>
					<Application />
				</BrowserRouter>
				<ToastContainer theme={current} position={"top-right"} />
			</ThemeProvider>
		</StyledEngineProvider>
	);
}

function App() {
	return (
		<DiProvider container={container}>
			<Provider store={store}>
				<Wrapper />
			</Provider>
		</DiProvider>
	);
}

const rootComponent = document.getElementById("root");
const root = createRoot(rootComponent!);
root.render(<App />);
