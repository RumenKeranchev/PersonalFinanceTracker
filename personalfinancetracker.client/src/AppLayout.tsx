import 'bootstrap/dist/css/bootstrap.min.css';
import { Container } from 'react-bootstrap';
import { Outlet } from 'react-router';
import './App.css';
import Sidebar from './Components/Shared/Sidebar';
import TopBar from './Components/Shared/TopBar';

function AppLayout() {
    return (
        <Container fluid className="p-0 min-vh-100 d-flex flex-column">
            <TopBar />
            <div className="d-flex flex-grow-1">
                <Sidebar />
                <section className="d-flex flex-grow-1 primary-color">
                    <Outlet />
                </section>
            </div>
        </Container>
    );
}

export default AppLayout;