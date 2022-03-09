import * as React from "react";
import "./Application.scss";
import Brightness5Icon from "@mui/icons-material/Brightness5";
import Brightness3Icon from "@mui/icons-material/Brightness3";
import { useAppDispatch, useAppSelector } from "../../store";
import { toggleTheme } from "../../store/module/theme/theme.action";
import { createDrawerAction, withDrawer } from "./utils/drawer/Drawer.hoc";
import { Box } from "@mui/material";
import { login, logout } from "../../store/module/authentication/authentication.action";
import { Login, Logout } from "@mui/icons-material";
import { Privacy } from "./privacy/Privacy";
import { Route, Routes } from "react-router-dom";

function Application() {
	const dispatch = useAppDispatch();

	const { theme, themeIcon, logged } = useAppSelector((s) => ({
		theme: s.theme.current,
		themeIcon: s.theme.current === "dark" ? <Brightness5Icon /> : <Brightness3Icon />,
		logged: s.authentication.logged,
	}));

	const actions = [
		createDrawerAction(theme === "dark" ? "Light Mode" : "Dark Mode", {
			icon: themeIcon,
			onClick: () => dispatch(toggleTheme()),
		}),
	];

	// if (logged) {
	// 	actions.push(
	// 		createDrawerAction("Logout", {
	// 			icon: <Logout fill={"currentColor"} />,
	// 			onClick: () => dispatch(logout()),
	// 		}),
	// 	);
	// } else {
	// 	actions.push(
	// 		createDrawerAction("Login", {
	// 			icon: <Login fill={"currentColor"} />,
	// 			onClick: () => dispatch(login()),
	// 		}),
	// 	);
	// }

	const drawer = withDrawer({
		component: <Routes>
			<Route path={"privacy"} element={<Privacy />} />
		</Routes>,
		actions,
		title: "ElyTransport",
	});

	return (
		<Box className={"Application"} bgcolor={"background.default"}>
			{drawer}
		</Box>
	);
}

export default Application;
