import { type ElementType, type ReactNode } from "react";

type FancyButtonProps<T extends ElementType> = {
  as?: T;
  children: ReactNode;
  className?: string;
} & React.ComponentPropsWithoutRef<T>;

function FancyButton<T extends ElementType = "button">({ as, className, children, ...rest }: FancyButtonProps<T>) {
    const FancyButton = as || "button";

    return (
        <FancyButton className={`primary-btn ${className ?? ""}`} {...rest}>
            <svg viewBox="0 0 100 40" preserveAspectRatio="none" className="fancySvg">
                <path
                    d="
                       M 0 0
                       L 5 0
                       L 5 5
                       L 7 5
                       L 7 0
                       L 100 0
                       L 100 28
                       L 95 40
                       L 0 40
                       Z"
                    vectorEffect="non-scaling-stroke"
                />
            </svg>
            <span>
                {children}
            </span>
        </FancyButton>
    );
};

export default FancyButton;