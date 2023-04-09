import { Line, Rows, Saparate } from "../../Layouts/Line/Line";
import { Button } from "../Button/Button";
import { DateTimeInput } from "../Input/DateTimeInput/DateTimeInput";
import { SelectInput } from "../Input/SelectInput/SelectInput";
import { Input } from "../Input/TextInput/TextInput";
import "./SearchMenu.css";
import React, { useState } from "react";

export const SearchMenu = ({ onButtonSearchClick, options }) => {
  let [dateTimeMin, SetDateTimeMin] = useState(new Date());
  let [dateTimeMax, SetDateTimeMax] = useState(new Date());
  let [selectedItem, setSelectedItem] = useState(null);
  let [text, setText] = useState("");
  return (
    <div className="SearchMenu">
      <Rows>
        <Line>
          <div>start time</div>
          <DateTimeInput onChange={(value) => SetDateTimeMin(value)} />
          <div>end time</div>
          <DateTimeInput onChange={(value) => SetDateTimeMax(value)} />
        </Line>
        <Saparate>
          <Line>
            <div>order by:</div>
            <SelectInput
              options={options || ["item 1", "item 2", "item 3"]}
              onChange={setSelectedItem}
            />
          </Line>
          <Line>
            <Input onChange={setText}></Input>
            <Button
              onClick={() =>
                onButtonSearchClick(
                  dateTimeMin,
                  dateTimeMax,
                  selectedItem,
                  text
                )
              }
            >
              Search
            </Button>
          </Line>
        </Saparate>
      </Rows>
    </div>
  );
};
