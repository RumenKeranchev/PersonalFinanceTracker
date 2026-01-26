import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { BrowserRouter, Route, Routes } from 'react-router'
import App from './App.tsx'
import Login from './Components/Auth/Login.tsx'
import Register from './Components/Auth/Register.tsx'
import { ToastProvider } from './Components/Shared/ToastContext.tsx'
import './index.css'

createRoot(document.getElementById('root')!).render(
    <StrictMode>
        <ToastProvider>
            <BrowserRouter>
                <Routes>
                    <Route path="/" element={<App />} >
                        <Route index element={<div>Dashboard</div>} />
                        <Route path="transactions" element={<div>Transactions</div>} />
                        <Route path="budgets" element={<div>Budgets</div>} />
                        <Route path="categories" element={<div>Categories</div>} />
                        <Route path="reports" element={<div>Reports</div>} />
                    </Route>
                    <Route path="login" Component={Login} />
                    <Route path="register" Component={Register} />
                </Routes>
            </BrowserRouter>
        </ToastProvider>
    </StrictMode>,
)
