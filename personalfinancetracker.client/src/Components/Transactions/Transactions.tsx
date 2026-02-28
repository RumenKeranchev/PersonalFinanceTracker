import { faBan, faFilter, faPlus } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import axios from "axios";
import { useEffect, useState } from "react";
import { TransactionType, type TransactionListItemDto } from "../../api";
import FancyButton from "../Shared/FancyButton";
import Item from "./Item";

const Transactions = () => {
    const [transactions, setTransactions] = useState<TransactionListItemDto[]>([]);

    useEffect(() => {
        const loadData = async () => {
            try {
                const response = await axios.get("/finance/transactions", {
                    params: {
                        "index": 0,
                        "size": 50
                    }
                });

                setTransactions(response.data.data);
            } catch {
                console.error()
            }
        };

        loadData();
    }, []);

    return (
        <div className="w-100">
            <div className="w-100 success p-1 d-flex gap-2 mt-1">
                <div className="flex-grow-1 ms-1">
                    <FancyButton as="button">
                        <FontAwesomeIcon icon={faPlus} /> New
                    </FancyButton>
                </div>
                <FancyButton>
                    <FontAwesomeIcon icon={faFilter} /> Filter
                </FancyButton>
                <FancyButton className="me-3">
                    <FontAwesomeIcon icon={faBan} /> Clear
                </FancyButton>
            </div>
            <div className="d-flex w-100 gap-2 p-2" style={{ height: "88.6vh", overflowY: "scroll" }}>
                <div className="d-flex flex-grow-1 flex-column gap-2">
                    {
                        transactions?.filter(t => t.type === TransactionType.Income).map(t => <Item key={t.amount} variant="income" amount={t.amount} date={t.date} />)
                    }
                </div>
                <div className="d-flex flex-grow-1 flex-column gap-2">
                    {
                        transactions?.filter(t => t.type === TransactionType.Transfer).map(t => <Item key={t.amount} variant="transfer" amount={t.amount} date={t.date} />)
                    }
                </div>
                <div className="d-flex flex-grow-1 flex-column gap-2">
                    {
                        transactions?.filter(t => t.type === TransactionType.Expense).map(t => <Item key={t.amount} variant="expense" amount={t.amount} date={t.date} />)
                    }
                </div>
            </div>
        </div>
    );
};

export default Transactions;