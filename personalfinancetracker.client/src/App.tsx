import 'bootstrap/dist/css/bootstrap.min.css';
import { Container } from 'react-bootstrap';
import './App.css';
//import Login from './Components/Auth/Login';
//import Register from './Components/Auth/Register';
import Sidebar from './Components/Shared/Sidebar';
import TopBar from './Components/Shared/TopBar';
import { Outlet } from 'react-router';

function App() {
    return (
        <Container fluid className="p-0 min-vh-100 d-flex flex-column">
            <TopBar />
            <div className="d-flex flex-grow-1">
                <Sidebar />
                <section className="d-flex flex-grow-1 primary-color">
                    {/*<div>*/}
                    {/*    <Register />*/}
                    {/*</div>*/}
                    {/*<div>*/}
                    {/*    <Login />*/}
                    {/*</div>*/}

                    <Outlet />
                </section>
            </div>
        </Container>
    );
}

export default App;