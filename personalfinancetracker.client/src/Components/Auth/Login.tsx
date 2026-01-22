import { useState } from "react";
import { Form } from "react-bootstrap";
import type { HttpValidationProblemDetails, LoginDto, ProblemDetails } from "../../api";
import { useToast } from "../Shared/ToastContext";

const Login = () => {
    const [model, setModel] = useState<LoginDto>({ email: "", password: "" });
    const [errors, setErrors] = useState<{ [key: string]: string[] }>();
    const { showToast } = useToast();

    const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        event.stopPropagation();

        const response = await fetch("https://localhost:7153/api/auth/login", {
            method: "post",
            headers: {
                "Content-Type": "application/json",
                "X-Api-Version": "1",
                "Client-Type": "browser",
            },
            credentials: "include",
            body: JSON.stringify(model)
        })

        if (!response.ok) {
            const problem = await response.json() as ProblemDetails;

            if ("errors" in problem) {
                const validation = problem as HttpValidationProblemDetails;
                setErrors(validation.errors);
                return;
            } else {
                setErrors({});
            }

            showToast({ message: problem.detail ?? problem.title ?? "Unknown error", variant: "danger" });
        }
    };

    return (
        <Form className="form" noValidate onSubmit={handleSubmit} style={{ width: 450 }}>
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
                Login
            </button>
        </Form>
    );
}

export default Login;