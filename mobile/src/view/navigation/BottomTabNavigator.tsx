import "react-native-gesture-handler";
import { FontAwesome5, MaterialIcons } from "@expo/vector-icons";
import { createMaterialTopTabNavigator } from "@react-navigation/material-top-tabs";
import * as React from "react";

import StationsScreen from "../screens/StationsScreen";
import LocationScreen from "../screens/TabTwoScreen";
import StoreScreen from "../screens/StoreScreen";
import { theme } from "../constants/Colors";
import linkingConfiguration from "./LinkingConfiguration";

export type Routes = typeof linkingConfiguration["config"]["screens"]["Root"]["screens"];

const BottomTab = createMaterialTopTabNavigator<Routes>();

export default function BottomTabNavigator() {
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
					tabBarIcon: ({ color }) => <FontAwesome5 size={24} name={"map-marked"} color={color} />,
				}}
			/>
			<BottomTab.Screen
				name="Location"
				component={LocationScreen}
				options={{
					tabBarIcon: ({ color }) => <MaterialIcons size={24} name="place" color={color} />,
				}}
			/>
			<BottomTab.Screen
				name="Data"
				component={StoreScreen}
				options={{
					tabBarIcon: ({ color }) => <FontAwesome5 size={24} name="database" color={color} />,
				}}
			/>
		</BottomTab.Navigator>
	);
}
