import { useState } from "react";
import type { HttpValidationProblemDetails, ProblemDetails, RegisterDto } from "../../api";
import { useToast } from "../Shared/ToastContext";

const Register = () => {
    const [model, setModel] = useState<RegisterDto>({ email: "", password: "", username: "" });
    const [errors, setErrors] = useState<{ [key: string]: string[] }>();
    const { showToast } = useToast();

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
            body: JSON.stringify(model),
            credentials: "include",
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
        <form className="form" noValidate onSubmit={handleSubmit} style={{ width: 450 }}>
            <div className="mb-3">
                <label>Email address</label>
                <input
                    type="email"
                    placeholder="Enter email"
                    required
                    onChange={(e) => setModel(prev => ({ ...prev, email: e.target?.value }))}
                    value={model?.email}
                    aria-invalid={!!errors?.Email}
                />
                {
                    errors?.Email && (
                        <div className="validation-error">
                            {errors.Email.map(e => <div key={e}>{e}</div>)}
                        </div>
                    )
                }
            </div>

            <div className="mb-3">
                <label>Username</label>
                <input
                    type="text"
                    placeholder="Enter username"
                    onChange={(e) => setModel(prev => ({ ...prev, username: e.target?.value }))}
                    value={model?.username}
                    aria-invalid={!!errors?.Username}
                />
                {
                    errors?.Username && (
                        <div className="validation-error">
                            {errors.Username.map(e => <div key={e}>{e}</div>)}
                        </div>
                    )
                }
            </div>

            <div className="mb-3">
                <label>Password</label>
                <input
                    type="password"
                    placeholder="Password"
                    required
                    onChange={(e) => setModel(prev => ({ ...prev, password: e.target?.value }))}
                    value={model?.password}
                    aria-invalid={!!errors?.Password}
                />
                {
                    errors?.Password && (
                        <div className="validation-error">
                            {errors.Password.map(e => <div key={e}>{e}</div>)}
                        </div>
                    )
                }
            </div>

            <button type="submit" className="primary-btn">
                Register
            </button>
        </form>
    );
}

export default Register;