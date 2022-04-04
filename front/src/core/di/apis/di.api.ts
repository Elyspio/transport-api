import { BackendApiClient } from "../../apis/backend";
import { AuthenticationApiClient } from "../../apis/authentication";
import { container } from "../index";

container.bind(BackendApiClient).toSelf();

container.bind(AuthenticationApiClient).toSelf();
