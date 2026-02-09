/* eslint-disable @typescript-eslint/no-explicit-any */
/* eslint-disable @typescript-eslint/no-unused-vars */
import axios from 'axios'
import 'bootstrap/dist/css/bootstrap.min.css'
import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { BrowserRouter, Navigate, Outlet, Route, Routes } from 'react-router'
import "tabulator-tables/dist/css/tabulator.min.css"
import AppLayout from './AppLayout.tsx'
import Login from './Components/Auth/Login.tsx'
import Register from './Components/Auth/Register.tsx'
import Home from './Components/Dashboard/Home.tsx'
import PublicLayout from './Components/PublicLayout.tsx'
import { AuthProvider, useAuth } from './Components/Shared/AuthContext.tsx'
import { ToastProvider } from './Components/Shared/ToastContext.tsx'
import TransactionsTable from './Components/Transactions/Table.tsx'
import '../styles/custom-tabulator.scss'

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

let isRefreshing = false;
let failedQueue: { resolve: (value?: any) => void; reject: (err: any) => void }[] = [];
const processQueue = (error: any) => {
    failedQueue.forEach(p => (error ? p.reject(error) : p.resolve()));
    failedQueue = [];
};

axios.defaults.withCredentials = true;
axios.defaults.baseURL = 'https://localhost:7153/api/v1/';
axios.defaults.headers.common['Client-Type'] = 'browser';
axios.defaults.headers.common['X-Api-Version'] = '1';

axios.interceptors.response.use(
    res => res, // pass through successful responses
    async err => {
        const originalRequest = err.config;

        // Only handle 401 errors
        if (err.response?.status === 401 && !originalRequest._retry) {
            originalRequest._retry = true;

            if (isRefreshing) {
                // queue the request until the token is refreshed
                return new Promise((resolve, reject) => {
                    failedQueue.push({ resolve, reject });
                }).then(() => {
                    return axios(originalRequest);
                });
            }

            isRefreshing = true;

            try {
                await axios.post('/auth/refresh');

                // retry the original request
                processQueue(null);

                return axios(originalRequest);
            } catch (refreshError) {
                processQueue(refreshError);
                return Promise.reject(refreshError);
            } finally {
                isRefreshing = false;
            }
        }

        return Promise.reject(err);
    }
);

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
                                <Route path="transactions" element={<TransactionsTable />} />
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
