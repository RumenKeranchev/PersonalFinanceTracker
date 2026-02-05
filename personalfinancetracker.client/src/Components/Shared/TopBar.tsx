import { faIdBadge } from "@fortawesome/free-regular-svg-icons";
import { faArrowRightToBracket } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { NavLink } from "react-router";
import { useAuth } from "./AuthContext";
import { faArrowRightFromBracket } from "@fortawesome/free-solid-svg-icons/faArrowRightFromBracket";
import axios from "axios";

const TopBar = () => {
    const { isAuthencticated, logout } = useAuth();

    const handleLogout = async () => {

        await axios.post("/auth/logout");
        logout();
    }

    return (
        <div className="top-bar">
            <h3>Personal Finance Tracker</h3>

            {
                !isAuthencticated &&
                <>
                    <NavLink to="/register" className={`primary-btn`} style={{ width: 130 }}><FontAwesomeIcon icon={faIdBadge} /> Register</NavLink>
                    <NavLink to="/login" className={`primary-btn`} style={{ width: 130 }}><FontAwesomeIcon icon={faArrowRightToBracket} /> Login</NavLink>
                </>
            }
            {
                isAuthencticated &&
                <>
                    <NavLink to="/profile" className={`primary-btn`} style={{ width: 130 }}><FontAwesomeIcon icon={faIdBadge} /> Profile</NavLink>
                    <button onClick={handleLogout} className={`primary-btn`} style={{ width: 130 }}><FontAwesomeIcon icon={faArrowRightFromBracket} /> Logout</button>
                </>
            }
        </div>
    );
};

export default TopBar;