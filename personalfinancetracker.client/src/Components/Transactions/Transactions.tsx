import { faBan, faPlus } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import axios from "axios";
import { useEffect, useState } from "react";
import { TransactionType, type TransactionListItemDto } from "../../api";
import FancyButton from "../Shared/FancyButton";
import Filter, { type FilterParams } from "./Filter";
import Item from "./Item";
import { AddNewModal } from "./AddNewModal";

const Transactions = () => {
    const [transactions, setTransactions] = useState<TransactionListItemDto[]>([]);
    const [filter, setFilter] = useState<FilterParams>({ amount: undefined, type: undefined, dateFrom: undefined, dateTo: undefined });
    const [showAddModal, setShowAddModal] = useState(false);

    useEffect(() => {
        const loadData = async () => {
            try {
                const filters = [];

                if (filter.amount)
                    filters.push({ field: "amount", operator: "equal", value: filter.amount });
                if (filter.type)
                    filters.push({ field: "type", operator: "equal", value: filter.type });
                if (filter.dateFrom)
                    filters.push({ field: "date", operator: "greaterThanOrEqual", value: filter.dateFrom.toISOString() });
                if (filter.dateTo)
                    filters.push({ field: "date", operator: "lessThanOrEqual", value: filter.dateTo.toISOString() });

                const response = await axios.get("/finance/transactions", {
                    params: {
                        "index": 0,
                        "size": 50,
                        "filters": JSON.stringify(filters)
                    }
                });

                setTransactions(response.data.data);
            } catch {
                console.error()
            }
        };

        loadData();
    }, [filter]);

    return (
        <div className="w-100">
            <div className="w-100 success p-1 d-flex gap-2 mt-1">
                <div className="flex-grow-1 ms-1">
                    <FancyButton onClick={() => setShowAddModal(true)}>
                        <FontAwesomeIcon icon={faPlus} /> New
                    </FancyButton>
                </div>
                <Filter filter={filter} setFilter={setFilter} />
                <FancyButton className="me-3" onClick={() => setFilter({ amount: undefined, type: undefined, dateFrom: undefined, dateTo: undefined })}>
                    <FontAwesomeIcon icon={faBan} /> Clear
                </FancyButton>
            </div>
            <div className="d-grid w-100 gap-2 p-2" style={{ height: "88.6vh", overflowY: "scroll", gridTemplateColumns: "repeat(3, 1fr)" }}>
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

            <AddNewModal
                show={showAddModal}
                onHide={() => setShowAddModal(false)} onExited={() => setFilter({ amount: undefined, type: undefined, dateFrom: undefined, dateTo: undefined })}
            />
        </div>
    );
};

export default Transactions;