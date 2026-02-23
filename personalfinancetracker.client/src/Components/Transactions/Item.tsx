import { DateTime } from "luxon";

const Item = (props: { variant: "income" | "expense" | "transfer", amount: number, date: Date }) => {
    return (
        <div className={`${props.variant} d-flex justify-content-around`}>
            <span>{props.amount.toFixed(2)}</span>
            <span>{DateTime.fromISO(props.date).toFormat("dd LLL yyyy HH:mm")}</span>
        </div>
    );
};

export default Item;