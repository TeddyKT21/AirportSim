import "./Table.css";
import React from "react";

const renderer = (item) => {
  if (
    typeof item === "string" &&
    item.includes(":") &&
    item.includes("-") &&
    item.includes("T")
  ) {
    let [date, time] = item.split("T");
    let [year, month, day] = date.split("-");
    let [hours, minutes, seconds] = time.split(":");
    seconds = String(Math.round(Number(seconds.split('+')[0])));
    if (seconds.length === 1) {
      seconds = "0" + seconds;
    }
    let dateStr = `${month}/${day}/${year} ${hours}:${minutes}:${seconds}`;
    return dateStr;
  }
  return item;
};
export const Table = ({ fields, dataMembers, headers = null }) => {
  let head = [];
  if (headers) {
    head = headers.map((header) => (
      <th key={headers.indexOf(header)}>{header}</th>
    ));
  } else {
    head = fields.map((field) => <th key={fields.indexOf(field)}>{field}</th>);
  }
  let rows = dataMembers.map((member) => (
    <tr key={member.id}>
      {fields.map((field) => (
        <td key={member.id + fields.indexOf(field)}>
          {renderer(member[field])}
        </td>
      ))}
    </tr>
  ));
  return (
    <div className="Table">
    <table>
      <thead>
        <tr>{head}</tr>
      </thead>
        <tbody>{rows}</tbody>
    </table>
    </div>
  );
};
