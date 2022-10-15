import * as Linking from "expo-linking";

export type LatLng = {
	longitude: number;
	latitude: number;
};

//https://www.waze.com/ul?to=ll.45.78127997%2C4.82977867&from=ll.45.7935651%2C4.8451191&utm_medium=lm_share_directions&utm_campaign=default&utm_source=waze_website

export class NavigationService {
	async openNavigation(destination: LatLng) {
		const url = `geo:${destination.latitude},${destination.longitude}`;
		await Linking.openURL(url);
	}
}
