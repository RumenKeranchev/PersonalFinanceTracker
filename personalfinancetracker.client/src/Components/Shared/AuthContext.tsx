import { createContext, useContext, useState, type ReactNode } from "react";


type AuthUser = {
    username: string
};

type AuthState = {
    user: AuthUser | null
    isAuthencticated: boolean,
};

type AuthContextValue = AuthState & {
    login: (user: AuthUser) => void
    logout: () => void
};

export const AuthContext = createContext<AuthContextValue | null>(null);

export function AuthProvider({ children }: { children: ReactNode }) {
    const userFromStorage = JSON.parse(localStorage.getItem("user") ?? "{}") as AuthUser;

    const [user, setUser] = useState<AuthUser | null>(userFromStorage);

    const login = (user: AuthUser) => {
        setUser(user);
        localStorage.setItem("user", JSON.stringify(user));
    };
    const logout = () => {
        setUser(null);
        localStorage.setItem("user", JSON.stringify({}));
    };

    const value: AuthContextValue = {
        user,
        isAuthencticated: user !== null,
        login,
        logout
    }

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