import axios from "axios";
import { useEffect, useState } from "react";
import { TransactionType, type TransactionListItemDto } from "../../api";
import Item from "./Item";

const Transactions = () => {
    const [transactions, setTransactions] = useState<TransactionListItemDto[]>([]);

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

    useEffect(() => { loadData() }, []);

    return (
        <div className="d-flex w-100 gap-2 p-2">
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
    );
};

export default Transactions;