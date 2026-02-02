/* eslint-disable @typescript-eslint/no-unused-vars */
import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { BrowserRouter, Outlet, Route, Routes } from 'react-router'
import AppLayout from './AppLayout.tsx'
import Login from './Components/Auth/Login.tsx'
import Register from './Components/Auth/Register.tsx'
import { AuthProvider, useAuth } from './Components/Shared/AuthContext.tsx'
import { ToastProvider } from './Components/Shared/ToastContext.tsx'
import './index.css'
import PublicLayout from './Components/PublicLayout.tsx'
import { Navigate } from 'react-router'
import Home from './Components/Dashboard/Home.tsx'

const RequireAuth = () => {
    const { isAuthencticated } = useAuth();

    if (!isAuthencticated) {
        return <Navigate to="/" replace />;
    }

    return <Outlet />;
};

const RedirectAuth = () => {
    const { isAuthencticated } = useAuth();

    if (isAuthencticated) {
        return <Navigate to="/dashboard" replace />;
    }

    return <Outlet />;
};

createRoot(document.getElementById('root')!).render(
    <StrictMode>
        <ToastProvider>
            <BrowserRouter>
                <AuthProvider>
                    <Routes>

                        <Route element={<RedirectAuth />}>
                            <Route element={<PublicLayout />}>
                                <Route index element={<h2 className="primary-color">Hello!</h2>} />
                                <Route path="login" element={<Login />} />
                                <Route path="register" element={<Register />} />
                            </Route>
                        </Route>

                        <Route element={<RequireAuth />}>
                            <Route element={<AppLayout />} >
                                <Route path="dashboard" element={<Home />} />
                                <Route path="transactions" element={<div>Transactions</div>} />
                                <Route path="budgets" element={<div>Budgets</div>} />
                                <Route path="categories" element={<div>Categories</div>} />
                                <Route path="reports" element={<div>Reports</div>} />
                            </Route>
                        </Route>

                    </Routes>
                </AuthProvider>
            </BrowserRouter>
        </ToastProvider>
    </StrictMode>,
)
