import type { PropsWithChildren } from "react";
import { Modal } from "react-bootstrap";
import FancyButton from "./FancyButton";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faBan } from "@fortawesome/free-solid-svg-icons";
import { faSave } from "@fortawesome/free-regular-svg-icons";

export type BaseModalProps = {
    show: boolean;
    onHide: () => void;
    onExited?: () => void;
};

type BaseModalType =
    | { type: "form", formId: string }
    | { type: "confirm" }
    | { type: "display" };

type FullModalProps = BaseModalProps & BaseModalType & PropsWithChildren
    & {
        title: string;
        className?: string;
        size?: "sm" | "lg" | "xl" | undefined;
    };

const BaseModal = ({ children, className, ...rest }: FullModalProps) => {
    return (
        <Modal {...rest} contentClassName={`form py-0 px-1 ${className}`} centered>
            <Modal.Header closeButton className="fs-5 cp-font">{rest.title}</Modal.Header>
            <Modal.Body>
                {children}
            </Modal.Body>
            <Modal.Footer>
                <div className="d-flex gap-2 justify-content-end">
                    <FancyButton onClick={rest.onHide}>
                        <FontAwesomeIcon icon={faBan} /> Cancel
                    </FancyButton>
                    <FancyButton onClick={rest.onHide} form={rest.type === "form" ? rest.formId : undefined}>
                        <FontAwesomeIcon icon={faSave} /> Save
                    </FancyButton>
                </div>
            </Modal.Footer>
        </Modal>
    );
};

export { BaseModal };
