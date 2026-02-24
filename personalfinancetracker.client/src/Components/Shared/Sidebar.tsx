import { faArrowRightArrowLeft, faChartLine, faCoins, faLayerGroup, faTv } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import FancyButton from "./FancyButton";
import { NavLink } from "react-router";

const Sidebar = () => {
    return (
        <aside className="sidebar">
            <FancyButton to="dashboard" style={{ width: "99%" }} as={NavLink}>
                <FontAwesomeIcon icon={faTv} className="" /> Dashboard
            </FancyButton>
            <FancyButton to="transactions" style={{ width: "99%" }} as={NavLink}>
                <FontAwesomeIcon icon={faArrowRightArrowLeft} className="" /> Transactions
            </FancyButton>
            <FancyButton to="budgets" style={{ width: "99%" }} as={NavLink}>
                <FontAwesomeIcon icon={faCoins} className="" /> Budgets
            </FancyButton>
            <FancyButton to="categories" style={{ width: "99%" }} as={NavLink}>
                <FontAwesomeIcon icon={faLayerGroup} className="" /> Categories
            </FancyButton>
            <FancyButton to="reports" style={{ width: "99%" }} as={NavLink}>
                <FontAwesomeIcon icon={faChartLine} className="" /> Reports
            </FancyButton>
        </aside>
    );
};

export default Sidebar;