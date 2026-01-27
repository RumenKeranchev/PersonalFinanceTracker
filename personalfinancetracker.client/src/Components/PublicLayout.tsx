import { Outlet } from "react-router";
import TopBar from "./Shared/TopBar";

const PublicLayout = () => {
    return (
        <div className="min-vh-100 d-flex flex-column">
            <TopBar /> {/*TODO: this needs to be register/login only*/}
            <main className="flex-grow-1 d-flex justify-content-center align-items-center">
                <Outlet />
            </main>
        </div>
    );
}

export default PublicLayout;