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

const RequireAuth = () => {
    const { isAuthencticated } = useAuth();

    if (!isAuthencticated) {
        return <Navigate to="/welcome" replace />
    }

    return <Outlet />;
}

createRoot(document.getElementById('root')!).render(
    <StrictMode>
        <ToastProvider>
            <BrowserRouter>
                <AuthProvider>
                    <Routes>

                        <Route element={<PublicLayout />}>
                            <Route path="welcome" element={<h2 className="primary-color">Hello!</h2>} />
                            <Route path="login" element={<Login />} />
                            <Route path="register" element={<Register />} />
                        </Route>

                        <Route element={<RequireAuth />}>
                            <Route element={<AppLayout />} >
                                <Route index element={<div>Dashboard</div>} />
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
