import 'bootstrap/dist/css/bootstrap.min.css';
import './App.css';
import Register from './Components/Auth/Register';
import Login from './Components/Auth/Login';
import Navigation from './Components/Shared/Navigation';
import { Container } from 'react-bootstrap';

function App() {
    return (
        <Container fluid className="p-0">
            <Navigation />
            <div className="d-flex gap-1" style={{ height: "100%" }}>
                <div style={{ width: 300, border: "10px solid" }}></div>
                <div className="d-flex gap-3">
                    <Register />
                    <div>
                        <Login />
                    </div>
                </div>
            </div>
        </Container>
    );
}

export default App;