import { NavLink } from "react-router";

const Sidebar = () => {
    return (
        <aside className="sidebar">
            <NavLink to="/" className={({ isActive }) => `${isActive ? "active" : ""} primary-btn`}>Dashboard</NavLink>
            <NavLink to="transactions" className={({ isActive }) => `${isActive ? "active" : ""} primary-btn`}>Transactions</NavLink>
            <NavLink to="budgets" className={({ isActive }) => `${isActive ? "active" : ""} primary-btn`}>Budgets</NavLink>
            <NavLink to="categories" className={({ isActive }) => `${isActive ? "active" : ""} primary-btn`}>Categories</NavLink>
            <NavLink to="reports" className={({ isActive }) => `${isActive ? "active" : ""} primary-btn`}>Reports</NavLink>
        </aside>
    );
};

export default Sidebar;