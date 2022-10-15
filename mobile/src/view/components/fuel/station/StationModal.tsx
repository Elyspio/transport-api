import React from "react";
import { AppModal } from "../../common/AppModal";
import { Box, Button, Stack, Text, useTheme } from "native-base";
import { FuelStationDataDistance } from "../../../../core/apis/backend/generated";
import { Services } from "../../../../core/services";
import { PriceValues } from "../../../constants/stations";
import { Price } from "../Price";
import { Feather } from "@expo/vector-icons";

type StationModalProps = {
	data: FuelStationDataDistance;
	modal: { set: (value: boolean) => void; get: boolean };
};

export function getDistance(val: number) {
	return val < 1000 ? `${val.toFixed(0)}m` : `${(val / 1000).toFixed(2)}Km`;
}

export function StationModal({ data, modal }: StationModalProps) {
	const onFuelStationClick = React.useCallback(() => {
		return Services.waze.openNavigation({
			longitude: data.location.longitude,
			latitude: data.location.latitude,
		});
	}, []);

	const prices = React.useMemo(() => {
		return Object.keys(data.prices)
			.filter((fuel) => data.prices[fuel as PriceValues].length)
			.map((fuel) => {
				const { date, value } = data.prices[fuel as PriceValues][0];
				return <Price key={fuel} value={value!} fuel={fuel as PriceValues} date={date} detailed />;
			});
	}, [data]);

	const theme = useTheme();

	return (
		<AppModal
			visible={modal.get}
			setVisible={modal.set}
			title={"Station Detail"}
			footer={
				<Stack direction={"row"} space={4} justifyContent={"center"} width={"100%"}>
					<Button borderRadius={"full"} variant={"outline"} onPress={onFuelStationClick}>
						<Feather color={theme.colors.tertiary["700"]} size={32} name={"navigation"} />
					</Button>
				</Stack>
			}
		>
			<Box>
				<Stack direction={"column"} space={3}>
					<Text opacity={0.8}>{data.location.address}</Text>
					<Text opacity={0.8}>City: {data.location.city}</Text>
					<Text opacity={0.8}>Distance: {getDistance(data.distance)}</Text>
					{prices}
				</Stack>
			</Box>
		</AppModal>
	);
}
