import { configureStore } from "@reduxjs/toolkit";
import { TypedUseSelectorHook, useDispatch, useSelector } from "react-redux";
import { themeReducer } from "./module/theme/theme.reducer";
import { statisticsReducer } from "./module/statistics/statistics.reducer";

const store = configureStore({
	reducer: {
		theme: themeReducer,
		statistic: statisticsReducer,
	},
	devTools: process.env.NODE_ENV !== "production",
	middleware: (getDefaultMiddleware) => getDefaultMiddleware({ serializableCheck: false }),
});

export type StoreState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;

export const useAppDispatch = () => useDispatch<AppDispatch>();
export const useAppSelector: TypedUseSelectorHook<StoreState> = useSelector;

export default store;
