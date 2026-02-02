import { faArrowRightArrowLeft, faChartLine, faCoins, faLayerGroup, faTv } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { NavLink } from "react-router";

const Sidebar = () => {
    return (
        <aside className="sidebar">
            <NavLink to="dashboard" className={({ isActive }) => `${isActive ? "active" : ""} primary-btn w-100`}>
                <FontAwesomeIcon icon={faTv} className="" /> Dashboard
            </NavLink>
            <NavLink to="transactions" className={({ isActive }) => `${isActive ? "active" : ""} primary-btn w-100`}>
                <FontAwesomeIcon icon={faArrowRightArrowLeft} className="" /> Transactions
            </NavLink>
            <NavLink to="budgets" className={({ isActive }) => `${isActive ? "active" : ""} primary-btn w-100`}>
                <FontAwesomeIcon icon={faCoins} className="" /> Budgets
            </NavLink>
            <NavLink to="categories" className={({ isActive }) => `${isActive ? "active" : ""} primary-btn w-100`}>
                <FontAwesomeIcon icon={faLayerGroup} className="" /> Categories
            </NavLink>
            <NavLink to="reports" className={({ isActive }) => `${isActive ? "active" : ""} primary-btn w-100`}>
                <FontAwesomeIcon icon={faChartLine} className="" /> Reports
            </NavLink>
        </aside>
    );
};

export default Sidebar;