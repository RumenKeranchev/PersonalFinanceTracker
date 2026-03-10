import { DateTime } from "luxon";
import FancyButton from "../Shared/FancyButton";

const Item = (props: { variant: "income" | "expense" | "transfer", amount: number, date: Date }) => {
    return (
        <FancyButton className={props.variant} >
            <div className={`d-flex justify-content-around`}>
                <span>{props.amount.toFixed(2)}</span>
                <span>{DateTime.fromISO(props.date as unknown as string).toLocal().toFormat("dd LLL yyyy HH:mm")}</span>
            </div>
        </FancyButton>
    );
};

export default Item;