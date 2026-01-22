import { Container, Nav, Navbar } from "react-bootstrap";

const Navigation = () => {
    return (
        <Navbar expand="lg" className="nav">
            <Container fluid>
                <Navbar.Brand>Personal Finance Tracker</Navbar.Brand>
                <Navbar.Toggle aria-controls="navbar-nav" />
                <Navbar.Collapse>
                    <Nav className="w-100">
                        <Nav.Link>Transactions</Nav.Link>
                        <Nav.Link>Budgets</Nav.Link>
                        <Nav.Link>Reports</Nav.Link>
                        <Nav.Link className="ms-auto">Register</Nav.Link>
                        <Nav.Link>Login</Nav.Link>
                    </Nav>
                </Navbar.Collapse>
            </Container>
        </Navbar>
    );
};

export default Navigation;