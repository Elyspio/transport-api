import * as Linking from "expo-linking";

export default {
	prefixes: [Linking.createURL("/")],
	config: {
		screens: {
			Root: {
				screens: {
					Fuel: {
						screens: {
							TabOneScreen: "one",
						},
					},
					Location: {
						screens: {
							TabTwoScreen: "two",
						},
					},
					Debug: {
						screens: {
							Data: "data",
						},
					},
				},
			},
			NotFound: "*",
		},
	},
};
