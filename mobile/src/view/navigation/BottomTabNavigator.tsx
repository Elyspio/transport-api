import "react-native-gesture-handler";
import { createMaterialTopTabNavigator } from "@react-navigation/material-top-tabs";
import * as React from "react";
import StationsScreen from "../screens/StationsScreen";
import StoreScreen from "../screens/StoreScreen";
import { theme } from "../constants/Colors";
import linkingConfiguration from "./LinkingConfiguration";
import { useAppSelector } from "../../store";
import { FontAwesome5 } from "@expo/vector-icons";

export type Routes = typeof linkingConfiguration["config"]["screens"]["Root"]["screens"];

const BottomTab = createMaterialTopTabNavigator<Routes>();

export default function BottomTabNavigator() {
	const { developerMode } = useAppSelector((s) => s.globals);

	return (
		<BottomTab.Navigator
			initialRouteName="Fuel"
			tabBarPosition={"bottom"}
			tabBarOptions={{
				activeTintColor: theme.colors.primary,
				inactiveTintColor: theme.colors.disabled,
				showIcon: true,
			}}
		>
			<BottomTab.Screen
				name="Fuel"
				component={StationsScreen}
				options={{
					tabBarIcon: ({ color }) => <FontAwesome5 size={24} name={"map-marked-alt"} color={color} />,
				}}
			/>

			{developerMode && (
				<BottomTab.Screen
					name="Debug"
					component={StoreScreen}
					options={{
						tabBarIcon: ({ color }) => <FontAwesome5 size={24} name="database" color={color} />,
					}}
				/>
			)}
		</BottomTab.Navigator>
	);
}
