import { useState } from "react";
import { Form, Button } from "react-bootstrap";

interface RegisterModel {
    email?: string;
    username?: string;
    password?: string;
}

const Register = () => {
    const [model, setModel] = useState<RegisterModel>({ email: "", password: "", username: "" });
    const [errors, setErrors] = useState({})

    const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        event.stopPropagation();

        const response = await fetch("https://localhost:7153/api/auth/register", {
            method: "post",
            headers: {
                "Content-Type": "application/json",
                "X-Api-Version": "1",
                "Client-Type": "browser",
            },
            body: JSON.stringify(model)
        })

        if (!response.ok) {
            const err = await response.json();
            setErrors(err.errors);
        }
    };

    return (
        <Form className="border rounded bg-light p-2" noValidate onSubmit={handleSubmit} style={{ minWidth: 450 }}>
            <Form.Group className="mb-3" controlId="formEmail">
                <Form.Label>Email address <span className="text-danger">*</span></Form.Label>
                <Form.Control
                    type="email"
                    placeholder="Enter email"
                    required
                    onChange={(e) => setModel(prev => ({ ...prev, email: e.target?.value }))}
                    value={model?.email}
                    isInvalid={!!errors?.Email}
                />
                <Form.Control.Feedback type="invalid">
                    {errors?.Email?.map((e, i) => <div key={i}>{e}</div>)}
                </Form.Control.Feedback>
            </Form.Group>

            <Form.Group className="mb-3" controlId="formUsername">
                <Form.Label>Username</Form.Label>
                <Form.Control
                    type="text"
                    placeholder="Enter username"
                    onChange={(e) => setModel(prev => ({ ...prev, username: e.target?.value }))}
                    value={model?.username}
                    isInvalid={!!errors?.Username}
                />
                <Form.Control.Feedback type="invalid">
                    {errors?.Username?.map((e, i) => <div key={i}>{e}</div>)}
                </Form.Control.Feedback>
            </Form.Group>

            <Form.Group className="mb-3" controlId="formPassword">
                <Form.Label>Password <span className="text-danger">*</span></Form.Label>
                <Form.Control
                    type="password"
                    placeholder="Password"
                    required
                    onChange={(e) => setModel(prev => ({ ...prev, password: e.target?.value }))}
                    value={model?.password}
                    isInvalid={!!errors?.Password}
                />
                <Form.Control.Feedback type="invalid">
                    {errors?.Password?.map((e, i) => <div key={i}>{e}</div>)}
                </Form.Control.Feedback>
            </Form.Group>

            <Button variant="primary" type="submit">
                Submit
            </Button>
        </Form>
    );
}

export default Register;