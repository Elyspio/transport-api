import { createSlice } from "@reduxjs/toolkit";
import { addDeveloperInput } from "./global.action";
import { ToastAndroid } from "react-native";

const initialState: State = {
	count: 0,
	developerMode: false,
};

type State = {
	count: number;
	developerMode: boolean;
};

const globalSlice = createSlice({
	name: "Location",
	reducers: {},
	initialState,
	extraReducers: ({ addCase }) => {
		addCase(addDeveloperInput, (state) => {
			state.count += 1;
			if (state.count === 5) ToastAndroid.show("5 more touch to enable developer mode", ToastAndroid.SHORT);
			if (state.count === 10) state.developerMode = true;
		});
	},
});

export const { reducer } = globalSlice;
