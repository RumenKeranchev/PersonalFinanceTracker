import { createContext, useContext, useEffect, useState, type ReactNode } from "react";
import { useNavigate } from "react-router";

type AuthUser = {
    username: string
};

type AuthState = {
    user: AuthUser | null
    isAuthencticated: boolean,
};

type AuthContextValue = AuthState & {
    login: (user: AuthUser) => void
    logout: () => void,
    isLoading: boolean
};

export const AuthContext = createContext<AuthContextValue | null>(null);

export function AuthProvider({ children }: { children: ReactNode }) {
    const userFromStorage = JSON.parse(localStorage.getItem("user") ?? "{}") as AuthUser;

    const [user, setUser] = useState<AuthUser | null>(userFromStorage);
    const [isLoading, setIsLoading] = useState(true);
    const navigate = useNavigate();

    const login = (user: AuthUser) => {
        setUser(user);
        localStorage.setItem("user", JSON.stringify(user));
        navigate("/", { replace: true });
    }

    const logout = () => {
        setUser(null);
        localStorage.setItem("user", JSON.stringify({}));
    };

    const value: AuthContextValue = {
        user,
        isAuthencticated: user?.username !== undefined,
        login,
        logout,
        isLoading
    };

    useEffect(() => {
        async function bootstrapAuth() {
            try {
                const res = await fetch("https://localhost:7153/api/auth/session", {
                    credentials: "include",
                    headers: {
                        "Client-Type": "browser"
                    }
                });

                if (!res.ok) throw new Error();

                const user = await res.json();
                login(user);
                localStorage.setItem("user", JSON.stringify(user));
                navigate("/", { replace: true });
            } catch {
                logout();
                localStorage.removeItem("user");
            } finally {
                setIsLoading(false);
            }
        }

        bootstrapAuth();
    // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    return (
        <AuthContext.Provider value={value}>
            {children}
        </AuthContext.Provider>
    );
}

export function useAuth() {
    const ctx = useContext(AuthContext);

    if (!ctx) {
        throw new Error("useAuth is outside of AuthProvider");
    }

    return ctx;
}