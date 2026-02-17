import { DateTime } from "luxon";
import { useEffect, useMemo, useRef } from "react";
import { Container } from "react-bootstrap";
import { TabulatorFull, type ColumnDefinition } from "tabulator-tables";
import { Table } from "../Table";
// eslint-disable-next-line @typescript-eslint/no-explicit-any
(window as any).luxon = DateTime;

const TransactionsTable = () => {
    const container = useRef<HTMLDivElement | null>(null);
    const table = useRef<TabulatorFull | undefined>(undefined);

    const columns = useMemo<ColumnDefinition[]>(() => [
        { title: "Amount", field: "amount", headerFilter: "number" },
        { title: "Type", field: "type", headerFilter: "input" },
        {
            title: "Date", field: "date", headerFilter: "datetime",
            formatter: "datetime",
            formatterParams: {
                inputFormat: "iso",
                outputFormat: "dd LLL yyyy HH:mm",
                timezone: Intl.DateTimeFormat().resolvedOptions().timeZone
            }
        }
    ], []);

    useEffect(() => {
        if (container.current) {
            table.current = new Table(container.current, "/finance/transactions", columns);

            // Cleanup
            return () => table.current?.destroy();
        }
    }, [columns]);

    return (
        <Container fluid className="px-0">
            <div ref={container}></div>
        </Container>
    );
};

export default TransactionsTable;