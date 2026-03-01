import { faFilter } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { DateTime } from 'luxon';
import { useRef, useState, type Dispatch, type MouseEventHandler, type SetStateAction } from 'react';
import { Overlay } from 'react-bootstrap';
import Popover from 'react-bootstrap/Popover';
import { TransactionType } from '../../api';
import FancyButton from '../Shared/FancyButton';

const Filter = (props: { filter: FilterParams, setFilter: Dispatch<SetStateAction<FilterParams>> }) => {
    const [show, setShow] = useState(false);
    const [target, setTarget] = useState(null);
    const ref = useRef(null);

    const handleClick: MouseEventHandler<HTMLButtonElement> = () => {
        setShow(!show);
        setTarget(ref.current);
    };

    return (
        <div ref={ref}>
            <FancyButton onClick={handleClick}>
                <FontAwesomeIcon icon={faFilter} /> Filter
            </FancyButton>

            <Overlay
                show={show}
                target={target}
                placement="bottom"
                container={ref}
                containerPadding={20}
                rootClose
                onHide={() => setShow(false)}
            >
                <Popover className="form w-100 d-flex flex-column gap-2">
                    <FancyButton as="div" className="w-100">
                        <input
                            type="number"
                            name="amount"
                            placeholder="Amount"
                            onChange={(e) => props.setFilter(prev => ({ ...prev, amount: parseFloat(e.target.value) }))}
                            value={props.filter.amount}
                        />
                    </FancyButton>
                    <FancyButton as="div" className="w-100">
                        <select
                            name="type"
                            value={props.filter.type}
                            onChange={(e) => props.setFilter(prev => ({ ...prev, type: e.target.value as TransactionType }))}
                        >
                            <option disabled selected>Type</option>
                            <option value={TransactionType.Income}>{TransactionType.Income}</option>
                            <option value={TransactionType.Transfer}>{TransactionType.Transfer}</option>
                            <option value={TransactionType.Expense}>{TransactionType.Expense}</option>
                        </select>
                    </FancyButton>
                    <FancyButton as="div" className="w-100">
                        <input
                            type="datetime-local"
                            name="date"
                            placeholder="Date"
                            onChange={(e) => props.setFilter(prev => ({ ...prev, date: DateTime.fromISO(e.target.value) }))}
                            value={props.filter.date?.toISO()?.slice(0, 16)}
                            defaultValue={DateTime.now().toISO().slice(0, 16)}
                        />
                    </FancyButton>
                </Popover>
            </Overlay>
        </div>
    );
};

export type FilterParams = {
    amount?: number,
    type?: TransactionType,
    date?: DateTime
};

export default Filter;