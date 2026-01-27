import { NavLink } from "react-router";

const TopBar = () => {
    return (
        <div className="top-bar">
            <h3>Personal Finance Tracker</h3>
            <NavLink to="/register" className={`primary-btn`}>Register</NavLink>
            <NavLink to="/login" className={`primary-btn`}>Login</NavLink>
        </div>
    );
};

export default TopBar;