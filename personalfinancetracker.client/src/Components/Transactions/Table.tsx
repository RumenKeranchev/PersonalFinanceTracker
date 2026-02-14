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
                    const tabulatorData = {
                        data: [],
                        last_page: 1,
                    };

                    try {
                        params["index"] -= 1;
                        const response = await axios.get(url, { params: params });

                        if (response.status === 200) {
                            tabulatorData.data = response.data;
                            tabulatorData.last_page = 100;
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
                autoColumns: true,
            });

            //table.on("rowClick", function (e, row) {
            //    alert("Row " + row.getData().id + " Clicked!!!!");
            //});

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