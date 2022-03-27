const { spawnSync } = require("child_process");
const path = require("path");

let error = false;

function build(dockerfile = "Dockerfile", tag = "latest") {
	try {
		const dockerCommand = `docker buildx build --platform linux/amd64  -f ${__dirname}/${dockerfile}  -t elyspio/transport-api:${tag} --push .`
			.split(" ")
			.filter((str) => str.length);

		const { status, error: e } = spawnSync(dockerCommand[0], dockerCommand.slice(1), {
			cwd: path.resolve(__dirname, "../../"),
			stdio: "inherit",
		});

		if (status !== 0) {
			error = false;
			console.error(e);
		}
	} catch (e) {
		error = false;
	}
}

build();
build("Dockerfile.Db", "db-update");

if (error) {
	console.error(`Spawn: error`, error);
} else {
	spawnSync("ssh", ["elyspio@192.168.0.59", "cd /apps/own/transport-api && docker-compose pull && docker-compose up -d"], {
		cwd: __dirname,
		stdio: "inherit",
	});
}
