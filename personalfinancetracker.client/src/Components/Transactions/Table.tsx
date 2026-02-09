import { useRef, useEffect, useMemo } from "react";
import { Container } from "react-bootstrap";
import { Tabulator } from "tabulator-tables";

const Table = () => {
    const container = useRef<HTMLDivElement | null>(null);

    const tabledata = useMemo(() => [
        { id: 1, name: "Oli Bob", age: "12", col: "red", dob: "" },
        { id: 2, name: "Mary May", age: "1", col: "blue", dob: "14/05/1982" },
        { id: 3, name: "Christine Lobowski", age: "42", col: "green", dob: "22/05/1982" },
        { id: 4, name: "Brendon Philips", age: "125", col: "orange", dob: "01/08/1980" },
        { id: 5, name: "Margret Marmajuke", age: "16", col: "yellow", dob: "31/01/1999" },
    ], []);

    useEffect(() => {
        if (container.current) {
            const table = new Tabulator(container.current, {
                height: "100%",
                data: tabledata,
                layout: "fitColumns",
                columns: [
                    { title: "Name", field: "name", width: 150 },
                    { title: "Age", field: "age", hozAlign: "left", formatter: "progress" },
                    { title: "Favourite Color", field: "col" },
                    { title: "Date Of Birth", field: "dob", sorter: "date" },
                ],
            });

            //table.on("rowClick", function (e, row) {
            //    alert("Row " + row.getData().id + " Clicked!!!!");
            //});

            // Cleanup
            return () => table.destroy();
        }
    }, [tabledata]);

    return (
        <Container fluid>
            <div ref={container}></div>
        </Container>
    );
};

export default Table;