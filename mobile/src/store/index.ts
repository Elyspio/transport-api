import { configureStore } from "@reduxjs/toolkit";
import { TypedUseSelectorHook, useDispatch, useSelector } from "react-redux";
import { reducer as locationReducer } from "./location/location.reducer";
import { reducer as stationsReducer } from "./stations/stations.reducer";

export const store = configureStore({
	devTools: true,
	reducer: {
		location: locationReducer,
		stations: stationsReducer,
	},
});

export type StoreState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;

export const useAppDispatch = () => useDispatch<AppDispatch>();
export const useAppSelector: TypedUseSelectorHook<StoreState> = useSelector;
