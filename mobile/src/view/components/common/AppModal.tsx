import { Center, Modal } from "native-base";
import React, { ReactNode } from "react";

type AppModalProps = {
	children: ReactNode;
	visible: boolean;
	setVisible: (state: boolean) => void;
	title?: string;
	footer?: ReactNode;
};

export function AppModal({ footer, title, visible, setVisible, children }: AppModalProps) {
	const setModal = React.useCallback((state: boolean) => () => setVisible(state), [setVisible]);

	return (
		<Modal isOpen={visible} onClose={setModal(false)} avoidKeyboard safeAreaTop={true}>
			<Center>
				<Modal.Content>
					<Modal.CloseButton />
					{title && <Modal.Header>{title}</Modal.Header>}

					<Modal.Body>{children}</Modal.Body>
					{footer && <Modal.Footer>{footer}</Modal.Footer>}
				</Modal.Content>
			</Center>
		</Modal>
	);
}
