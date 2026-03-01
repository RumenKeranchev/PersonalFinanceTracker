/* eslint-disable @typescript-eslint/no-explicit-any */
import { useLayoutEffect, useRef, useState, type ElementType } from "react";

type BaseProps<T extends ElementType> = {
    as?: T;
    className?: string;
};

type FancyButtonProps<T extends ElementType> = BaseProps<T>
    & React.ComponentPropsWithoutRef<T>                     // inject props per element type. if no 'as' is provided, default to button props
    & React.PropsWithChildren<BaseProps<T>>;                // inject the children prop into FancyButtonProps

function FancyButton<T extends ElementType = "button">({ as, className, children, ...rest }: FancyButtonProps<T>) {
    const Component = as || "button";
    const ref = useRef<any>(null);

    /* ── Size of the button in real pixels ── */
    const [size, setSize] = useState<DOMRect | null>(null);

    useLayoutEffect(() => {
        if (!ref.current) return;

        const ro = new ResizeObserver((entries) => {
            setSize(entries[0].contentRect);
        });

        ro.observe(ref.current);

        return () => ro.disconnect();
    }, []);

    const w = size?.width ?? 104;
    const h = size?.height ?? 42;
    const d = Math.min(18, h * 0.35);

    const notch = Math.min(h * 0.3, 12);
    const notchDepth = notch * 0.4;
    const notchWidth = notch * 1.8;

    return (
        <Component className={`primary-btn ${className ?? ""}`} {...rest} ref={ref}>
            <svg viewBox={`0 0 ${size?.width ?? 100} ${size?.height ?? 40}`} preserveAspectRatio="none" className="fancySvg">
                <path
                    d={`
                       M 0 0
                       L ${notch} 0
                       L ${notch} ${notchDepth}
                       L ${notchWidth} ${notchDepth}
                       L ${notchWidth} 0
                       L ${w} 0
                       L ${w} ${h - d}
                       L ${w - d} ${h}
                       L 0 ${h}
                       Z`}
                    vectorEffect="non-scaling-stroke"
                />
            </svg>
            {
                children != null &&
                <span>
                    {children}
                </span>
            }
        </Component>
    );
};

export default FancyButton;