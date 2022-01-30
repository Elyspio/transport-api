import { createAction, createReducer } from "@reduxjs/toolkit";

export const add = createAction<number>("add");

export type ExampleState = {
	value: number;
};

const defaultState = {
	value: 0,
};

export const exampleReducer = createReducer<ExampleState>(defaultState, ({ addCase }) => {
	addCase(add, (state, action) => {
		state.value += action.payload;
	});
});
