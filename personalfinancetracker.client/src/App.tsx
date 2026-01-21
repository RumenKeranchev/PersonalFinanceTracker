import 'bootstrap/dist/css/bootstrap.min.css';
import './App.css';
import Register from './Components/Auth/Register';
import Login from './Components/Auth/Login';

function App() {
    return (
        <div className="d-flex gap-3">
            <Register />
            <div>
                <Login />
            </div>
        </div>
    );
}

export default App;