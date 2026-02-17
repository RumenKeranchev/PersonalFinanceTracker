import axios from "axios";
import { TabulatorFull, type ColumnDefinition } from "tabulator-tables";

export class Table extends TabulatorFull {
    constructor(element: HTMLElement, url: string, columns: ColumnDefinition[]) {
        super(element, {
            height: "100%",
            ajaxURL: url,
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
            columns: columns,
            columnDefaults: {
                headerFilterLiveFilter: false,
            },
            renderVertical: "basic",
        });
    }
}