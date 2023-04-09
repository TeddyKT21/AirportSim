import { useState } from "react";
import DateTimePicker from "react-datetime-picker";
import React from "react";
import "./DateTimeInput.css";

export const DateTimeInput = ({ onChange, children }) => {
    let [value,SetValue] = useState(new Date());
  return (
    <DateTimePicker
      disableClock={true}
      onChange={(value) => {
        SetValue(value)
        onChange(value);
    }}
      value={value}
    />
  );
};
