import { useState, type FormEventHandler } from "react";
import { BaseModal, type BaseModalProps } from "../Shared/BaseModal";
import FancyButton from "../Shared/FancyButton";
import { TransactionType, type HttpValidationProblemDetails, type ProblemDetails, type TransactionCreateDto } from "../../api";
import { DateTime } from "luxon";
import axios, { AxiosError } from "axios";
import { useToast } from "../Shared/ToastContext";

const AddNewModal = (props: BaseModalProps) => {
    const formId = "addNewTransaction";
    const [errors, setErrors] = useState<{ [key: string]: string[] }>();
    const [model, setModel] = useState<TransactionCreateDto>({ amount: 0, type: TransactionType.Transfer, description: "", date: new Date() });
    const { showToast } = useToast();

    const handleSubmit: FormEventHandler<HTMLFormElement> = async (event) => {
        event.preventDefault();
        event.stopPropagation();

         try {
             const response = await axios.post("/finance/transactions", model);

             if (response.status === 204) {
                 props.onHide();

                 if (props.onExited) {
                     props.onExited();
                 }
             }

        } catch (e) {
            const error = e as AxiosError;
            const problem = error.response?.data as ProblemDetails;

            if (problem && "errors" in problem) {
                const validation = problem as HttpValidationProblemDetails;
                setErrors(validation.errors);
                return;
            } else {
                setErrors({});
            }

            showToast({ message: problem?.detail ?? problem?.title ?? "Unknown error", variant: "danger" });
        }
    }

    return (
        <BaseModal {...props}
            type="form"
            formId={formId}
            title="New Transaction"
        >
            <form id={formId} className="d-flex flex-column gap-3" onSubmit={handleSubmit}>
                <FancyButton as="div" className="py-2 w-100">
                    <input
                        type="number"
                        placeholder="Amount"
                        required
                        onChange={(e) => setModel(prev => ({ ...prev, amount: parseFloat(e.target?.value) }))}
                        value={model?.amount}
                        aria-invalid={!!errors?.Amount}
                    />
                    {
                        errors?.Amount && (
                            <div className="validation-error">
                                {errors.Amount.map((e: string) => <div key={e}>{e}</div>)}
                            </div>
                        )
                    }
                </FancyButton>

                <FancyButton as="div" className="py-2 w-100">
                    <input
                        type="datetime-local"
                        placeholder="Date"
                        required
                        onChange={(e) => setModel(prev => ({ ...prev, date: DateTime.fromISO(e.target.value).toJSDate() }))}
                        value={model.date ? DateTime.fromJSDate(new Date(model.date)).toFormat("yyyy-MM-dd'T'HH:mm") : ""}
                        aria-invalid={!!errors?.Date}
                    />
                    {
                        errors?.Date && (
                            <div className="validation-error">
                                {errors.Date.map((e: string) => <div key={e}>{e}</div>)}
                            </div>
                        )
                    }
                </FancyButton>

                <FancyButton as="div" className="py-2 w-100">
                    <select
                        required
                        onChange={(e) => setModel(prev => ({ ...prev, type: e.target?.value as TransactionType }))}
                        value={model.type}
                        aria-invalid={!!errors?.Type}
                    >
                        <option value={TransactionType.Income}>{TransactionType.Income}</option>
                        <option value={TransactionType.Transfer}>{TransactionType.Transfer}</option>
                        <option value={TransactionType.Expense}>{TransactionType.Expense}</option>
                    </select>
                    {
                        errors?.Type && (
                            <div className="validation-error">
                                {errors.Type.map((e: string) => <div key={e}>{e}</div>)}
                            </div>
                        )
                    }
                </FancyButton>

                <FancyButton as="div" className="py-2 w-100">
                    <input
                        type="text"
                        placeholder="Description"
                        onChange={(e) => setModel(prev => ({ ...prev, description: e.target?.value }))}
                        value={model?.description}
                        aria-invalid={!!errors?.Description}
                    />
                    {
                        errors?.Description && (
                            <div className="validation-error">
                                {errors.Description.map((e: string) => <div key={e}>{e}</div>)}
                            </div>
                        )
                    }
                </FancyButton>
            </form>
        </BaseModal>
    );
};

export { AddNewModal };