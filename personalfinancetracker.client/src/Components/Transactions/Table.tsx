import axios from "axios";
import { useEffect, useRef } from "react";
import { Container } from "react-bootstrap";
import { TabulatorFull } from "tabulator-tables";

const Table = () => {
    const container = useRef<HTMLDivElement | null>(null);
    const table = useRef<TabulatorFull | undefined>(undefined);

    // static sample data removed; using server-side data via ajax

    useEffect(() => {
        if (container.current) {
            table.current = new TabulatorFull(container.current, {
                height: "100%",
                ajaxURL: "/finance/transactions",
                pagination: true,
                paginationMode: "remote",
                paginationSize: 20,
                filterMode: "remote",
                sortMode: "remote",
                ajaxRequestFunc: async (url, config, params) => {
                    let tabulatorData = {
                        data: [],
                        last_page: 1,
                    };

                    try {
                        params["index"] -= 1;
                        const response = await axios.get(url, {
                            params: {
                                index: params["index"],
                                size: params["size"],
                                filters: JSON.stringify(params["filter"]),
                                sorters: JSON.stringify(params["sort"]),
                            }
                        });

                        if (response.status === 200) {
                            tabulatorData = response.data;
                        }

                    } catch (e) {
                        console.error(e);
                    }

                    return tabulatorData;
                },
                dataSendParams: {
                    "page": "index"
                },
                layout: "fitColumns",
                columns: [
                    { title: "Amount", field: "amount", headerFilter: "number" },
                    { title: "Type", field: "type", headerFilter: "input" },
                    {
                        title: "Date", field: "date", headerFilter: "datetime",
                        formatter: (cell) => {
                            let date = cell.getValue() as string;
                            date = date.substring(0, 23);

                            return new Date(date).toLocaleString("en-GB", { day: "2-digit", month: "short", year: "numeric", hour: "2-digit", hourCycle: "h24", minute: "2-digit" });
                        }
                    },
                ],
            });

            // Cleanup
            return () => table.current?.destroy();
        }
    }, []);

    return (
        <Container fluid className="px-0">
            <div ref={container}></div>
        </Container>
    );
};

export default Table;