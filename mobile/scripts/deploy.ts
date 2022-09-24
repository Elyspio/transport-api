import { expo } from "../app.json";
import * as fs from "fs";
import * as path from "path";
import { platform } from "os";
import { spawnSync } from "child_process";
import semver from "semver";

const isWindows = () => platform() === "win32";

const getNodeModulesBin = (name: string) => path.resolve(__dirname, "..", "node_modules", ".bin", isWindows() ? `${name}.cmd` : name);

const getOldConfig = () => JSON.parse(JSON.stringify(expo)) as typeof expo;

const config = getOldConfig();

const setAppJson = (conf: any) => {
	fs.writeFileSync(path.resolve(__dirname, "..", "app.json"), JSON.stringify({ expo: conf }, null, 4).replaceAll("    ", "\t"));
};

const now = new Date();

try {
	config.android.versionCode++;
	const version = semver.parse(config.version)!;
	version.inc("patch", "1");
	config.version = version.toString();
	setAppJson(config);
	const output = spawnSync("docker", ["compose", "up", "--build"], {
		cwd: path.resolve(__dirname, "..", "..", "deploy", "build", "mobile"),
		stdio: "inherit",
	});
	if (output.error) throw output.error;
	console.log(output.output?.toString());
} catch (e) {
	console.error(e);
	setAppJson(getOldConfig());
}
