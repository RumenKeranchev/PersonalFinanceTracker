import { createContext, useCallback, useContext, useState } from "react";
import { Toast, ToastContainer } from "react-bootstrap";
import type { Variant } from "react-bootstrap/esm/types";

export interface AppToast {
    id: string;
    message: string;
    variant: Variant;
}

interface AppToastCtxValue {
    showToast: (toast: Omit<AppToast, "id">) => void
}

const AppToastContext = createContext<AppToastCtxValue | undefined>(undefined);

export function ToastProvider({ children }: { children: React.ReactNode }) {
    const [toasts, setToasts] = useState<AppToast[]>([]);

    const show = useCallback((toast: Omit<AppToast, "id">) => {
        setToasts(prev => ([...prev,
        {
            id: crypto.randomUUID(),
            ...toast
        }
        ]));
    }, []);

    const removeToast = (id: string) => setToasts(prev => prev.filter(t => t.id !== id));

    return (
        <AppToastContext.Provider value={{ showToast: show }}>
            {children}

            <ToastContainer position="top-center" className="p-3">
                {toasts.map(t => (
                    <Toast
                        key={t.id}
                        bg={t.variant}
                        delay={4000}
                        autohide
                        onClose={() => removeToast(t.id)}
                    >
                        <Toast.Body>{t.message}</Toast.Body>
                    </Toast>
                ))}
            </ToastContainer>

        </AppToastContext.Provider>
    );
}

export function useToast() {
    const ctx = useContext(AppToastContext);
    if (!ctx) {
        throw new Error("useToast must be used within ToastProvider");
    }
    return ctx;
}
