import { ThemeService } from "../../services/theme.service";
import { LocalStorageService } from "../../services/localStorage.service";
import { DiKeysService } from "./di.keys.service";
import { container } from "../index";
import { StatisticsService } from "../../services/statistics.service";
import { LocationsService } from "../../services/locations.service";

container.bind(ThemeService).toSelf();
container.bind(StatisticsService).toSelf();
container.bind(LocationsService).toSelf();
container.bind<LocalStorageService>(DiKeysService.localStorage.settings).toConstantValue(new LocalStorageService("elyspio-authentication-settings"));

container.bind<LocalStorageService>(DiKeysService.localStorage.validation).toConstantValue(new LocalStorageService("elyspio-authentication-validation"));
