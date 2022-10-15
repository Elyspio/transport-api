import * as React from "react";
import { PropsWithChildren } from "react";
import { Prices } from "../../../core/apis/backend/generated";
import { useAppSelector } from "../../../store";
import { Icon, ITextProps, Stack, Text, useToast } from "native-base";
import { MaterialCommunityIcons } from "@expo/vector-icons";
import dayjs from "dayjs";

type PriceProps = {
	value: number;
	date: string;
	fuel: keyof Prices;
	detailed?: boolean;
};

export function Price({ value, fuel, date, detailed }: PriceProps) {
	const lower = useAppSelector((s) => s.stations.lowest);

	const toast = useToast();

	const delta = React.useMemo(() => (lower ? value - (lower[fuel] ?? 0) : null), [lower]);

	const showToast = React.useCallback(
		(text: string) => () =>
			toast.show({
				description: text,
				duration: 2.5,
			}),
		[toast]
	);

	const color = React.useMemo(() => {
		if (delta === null) return;
		if (delta === 0) return "#0F0";
		if (delta < 0.05) return "#BF0";
		if (delta < 0.1) return "#FF0";
		if (delta < 0.15) return "#FB0";
		if (delta < 0.2) return "#F90";
		return "#F00";
	}, [delta]);

	let dateStr = React.useMemo(
		() =>
			new Date(date).toLocaleDateString("FR-fr", {
				hour: "2-digit",
				minute: "2-digit",
			}),
		[date]
	);
	let dateWarning = React.useMemo(
		() =>
			dayjs(date).add(1, "day").isBefore(dayjs()) ? (
				<Icon onTouchStart={showToast("Price is older than one day")}>
					<MaterialCommunityIcons size={15} color={"#FFAA00"} name={"alert"} />
				</Icon>
			) : null,
		[date]
	);
	return React.useMemo(
		() => (
			<Stack space={4} direction={"row"}>
				<TextLabel width={59}>{fuel.toLocaleUpperCase()}</TextLabel>
				<TextLabel>{value.toFixed(3)} €</TextLabel>
				<TextLabel color={color}>{delta ? `${delta.toFixed(3)}€` : "             "}</TextLabel>
				{detailed && (
					<Stack direction={"row"} space={2} alignItems={"center"}>
						<TextLabel>{dateStr}</TextLabel>
						{dateWarning}
					</Stack>
				)}
			</Stack>
		),
		[color, value, fuel, delta]
	);
}

const TextLabel = ({ children, width, ...others }: PropsWithChildren<ITextProps>) => {
	return (
		<Text color={"#929292"} width={width} {...others}>
			{children}
		</Text>
	);
};
