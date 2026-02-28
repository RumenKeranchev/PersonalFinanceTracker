import { faIdBadge } from "@fortawesome/free-regular-svg-icons";
import { faArrowRightToBracket } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { NavLink } from "react-router";
import { useAuth } from "./AuthContext";
import { faArrowRightFromBracket } from "@fortawesome/free-solid-svg-icons/faArrowRightFromBracket";
import axios from "axios";
import FancyButton from "./FancyButton";

const TopBar = () => {
    const { isAuthencticated, logout } = useAuth();

    const handleLogout = async () => {

        await axios.post("/auth/logout");
        logout();
    }

    return (
        <div className="top-bar">
            <h3 className="logo">Personal Finance Tracker</h3>

            {
                !isAuthencticated &&
                <>
                    <FancyButton to="/register" style={{ width: 130 }} as={NavLink}>
                        <FontAwesomeIcon icon={faIdBadge} /> Register
                    </FancyButton>
                    <FancyButton to="/login" className={`primary-btn`} style={{ width: 130 }} as={NavLink}>
                        <FontAwesomeIcon icon={faArrowRightToBracket} /> Login
                    </FancyButton>
                </>
            }
            {
                isAuthencticated &&
                <>
                    <FancyButton to="/profile" style={{ width: 130 }} as={NavLink}>
                        <FontAwesomeIcon icon={faIdBadge} /> Profile
                    </FancyButton>
                    <FancyButton onClick={handleLogout} style={{ width: 130 }}>
                        <FontAwesomeIcon icon={faArrowRightFromBracket} /> Logout
                    </FancyButton>
                </>
            }
        </div>
    );
};

export default TopBar;