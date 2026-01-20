import { useState } from "react";
import { Form } from "react-bootstrap";
import type { HttpValidationProblemDetails, ProblemDetails } from "../../api";

interface RegisterModel {
    email?: string;
    username?: string;
    password?: string;
}

const Register = () => {
    const [model, setModel] = useState<RegisterModel>({ email: "", password: "", username: "" });
    const [errors, setErrors] = useState<{ [key: string]: string[] }>();

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
            const problem = await response.json() as ProblemDetails;

            if ("errors" in problem) {
                const validation = problem as HttpValidationProblemDetails;
                setErrors(validation.errors);
                return;
            }

            alert(problem.detail ?? problem.title ?? "Unknown error");
        }
    };

    return (
        <Form className="form" noValidate onSubmit={handleSubmit} style={{ minWidth: 450 }}>
            <Form.Group className="mb-3" controlId="formEmail">
                <Form.Label>Email address</Form.Label>
                <Form.Control
                    type="email"
                    placeholder="Enter email"
                    required
                    onChange={(e) => setModel(prev => ({ ...prev, email: e.target?.value }))}
                    value={model?.email}
                    isInvalid={!!errors?.Email}
                />
                <Form.Control.Feedback type="invalid">
                    {errors?.Email?.map((e: string, i: number) => <div key={i}>{e}</div>)}
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
                    {errors?.Username?.map((e: string, i: number) => <div key={i}>{e}</div>)}
                </Form.Control.Feedback>
            </Form.Group>

            <Form.Group className="mb-3" controlId="formPassword">
                <Form.Label>Password</Form.Label>
                <Form.Control
                    type="password"
                    placeholder="Password"
                    required
                    onChange={(e) => setModel(prev => ({ ...prev, password: e.target?.value }))}
                    value={model?.password}
                    isInvalid={!!errors?.Password}
                />
                <Form.Control.Feedback type="invalid">
                    {errors?.Password?.map((e: string, i: number) => <div key={i}>{e}</div>)}
                </Form.Control.Feedback>
            </Form.Group>

            <button type="submit" className="primary-btn">
                Submit
            </button>
        </Form>
    );
}

export default Register;