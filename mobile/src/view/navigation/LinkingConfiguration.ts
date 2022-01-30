import * as Linking from "expo-linking";

export default {
	prefixes: [Linking.makeUrl("/")],
	config: {
		screens: {
			Root: {
				screens: {
					Stations: {
						screens: {
							TabOneScreen: "one",
						},
					},
					Location: {
						screens: {
							TabTwoScreen: "two",
						},
					},
					Data: {
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
