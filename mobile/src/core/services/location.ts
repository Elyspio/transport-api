import * as Location from "expo-location";
import * as TaskManager from "expo-task-manager";

let taskName = "LOCATION_TASK_NAME";

export class LocationService {
	private callbacks = Array<(location: Location.LocationObject) => any>();

	public subscribe(callback: (loc: Location.LocationObject) => any) {
		this.callbacks.push(callback);
	}

	public async askPermissions() {
		let { status } = await Location.requestPermissionsAsync();
		return status !== "granted";
	}

	async start() {
		await Location.startLocationUpdatesAsync(taskName, {
			accuracy: Location.Accuracy.High,
			activityType: Location.ActivityType.AutomotiveNavigation,
			foregroundService: {
				notificationTitle: "notificationTitle",
				notificationBody: "notificationBody",
				notificationColor: "#0F007C",
			},
		});

		TaskManager.defineTask(taskName, ({ data, error }) => {
			if (error) {
				console.error(error);
				// check `error.message` for more details.
				return;
			}

			const location = data;
			console.log(location);

			// console.log('Received new locations', data.locatio);
			// this.emit(locations[0])
		});
	}

	stop() {
		return Location.stopLocationUpdatesAsync(taskName);
	}

	isRunning() {
		return Location.hasStartedLocationUpdatesAsync(taskName);
	}

	private emit(location: Location.LocationObject) {
		return Promise.all(this.callbacks.map((c) => c(location)));
	}
}
