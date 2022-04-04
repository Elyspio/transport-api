type LoggerName = string | Function | { constructor: { name: string } };

export function getLogger(name: LoggerName, type?: "Middleware" | "Service" | "Controller" | "Repository") {
	const nameType = typeof name;
	if (nameType === "object" && name.constructor?.name) {
		name = name.constructor.name;
	} else if (nameType !== "string") {
		name = (name as Function).name;
	}

	return {
		log: (...args: any[]) => console.log(`${type ? `${type} - ` : ""}${name}`, ...args),
		error: (...args: any[]) => console.error(`${type ? `${type} - ` : ""}${name}`, ...args),
		debug: (...args: any[]) => console.debug(`${type ? `${type} - ` : ""}${name}`, ...args),
		info: (...args: any[]) => console.info(`${type ? `${type} - ` : ""}${name}`, ...args),
		warn: (...args: any[]) => console.warn(`${type ? `${type} - ` : ""}${name}`, ...args),
	};
}

getLogger.service = (name: LoggerName) => getLogger(name, "Service");
getLogger.repository = (name: LoggerName) => getLogger(name, "Repository");
getLogger.middleware = (name: LoggerName) => getLogger(name, "Middleware");
getLogger.controller = (name: LoggerName) => getLogger(name, "Controller");
getLogger.default = () => getLogger("Default", undefined);
