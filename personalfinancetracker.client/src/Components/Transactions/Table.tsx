import axios from "axios";
import { useEffect, useRef } from "react";
import { Container } from "react-bootstrap";
import { TabulatorFull } from "tabulator-tables";
import { DateTime } from "luxon";
// eslint-disable-next-line @typescript-eslint/no-explicit-any
(window as any).luxon = DateTime;

const Table = () => {
    const container = useRef<HTMLDivElement | null>(null);
    const table = useRef<TabulatorFull | undefined>(undefined);

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
                        formatter: "datetime",
                        formatterParams: {
                            inputFormat: "iso",
                            outputFormat: "dd LLL yyyy HH:mm",
                            timezone: Intl.DateTimeFormat().resolvedOptions().timeZone
                        }
                    }
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