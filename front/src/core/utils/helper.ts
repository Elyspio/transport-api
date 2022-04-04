export namespace Helper {
	export const isDev = () => process.env.NODE_ENV !== "production";

	export function getCurrentFunctionName(skipOne: boolean) {
		return new Error()
			.stack!.split("\n")
			[2 + (skipOne ? 1 : 0)] // " at functionName ( ..." => "functionName"
			.replace(/^\s+at\s+(.+?)\s.+/g, "$1");
	}

	export function getFunctionArgs(func: Function) {
		return (func + "")
			.replace(/[/][/].*$/gm, "") // strip single-line comments
			.replace(/\s+/g, "") // strip white space
			.replace(/[/][*][^/*]*[*][/]/g, "") // strip multi-line comments
			.split("){", 1)[0]
			.replace(/^[^(]*[(]/, "") // extract the parameters
			.replace(/=[^,]+/g, "") // strip any ES6 defaults
			.split(",")
			.filter(Boolean); // split & filter [""]
	}

	export function deepEqual(obj1: any, obj2: any) {
		if (obj1 === obj2) {
			return true;
		} else if (isObject(obj1) && isObject(obj2)) {
			if (Object.keys(obj1).length !== Object.keys(obj2).length) {
				return false;
			}
			for (const prop in obj1) {
				if (!deepEqual(obj1[prop], obj2[prop])) {
					return false;
				}
			}
			return true;
		}

		// Private
		function isObject(obj: any) {
			return typeof obj === "object" && obj != null;
		}
	}

	export function getEnumValues(Enum: any): string[] {
		return Object.values(Enum);
	}

	export function getEnumValue<T>(Enum: any, val: string | number): T {
		for (const [key, value] of Object.entries(Enum)) {
			if (value === val) return Enum[key] as T;
		}
		throw new Error(`${val} is not in ${Enum}`);
	}
}
