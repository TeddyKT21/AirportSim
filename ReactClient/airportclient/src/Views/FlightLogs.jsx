import { SearchMenu } from "../UIkit/Elements/SearchMenu/SerchMenu";
import { Table } from "../UIkit/Elements/Table/Table";
import { Rows } from "../UIkit/Layouts/Line/Line";
import "./Views.css";
import React, { useState } from "react";
import { UseFetch } from "../CustomHooks/FetchEffect";
import { OnSearch } from "./OnSearch";

export const FlightLogs = (props) => {
  let [logs, setLogs] = useState(null);
  function preProccess(logs) {
    if (!logs) {
      return;
    }
    logs = logs.map((log) => {
      let wordsContent = log.text.split(" ");
      log.flight = Number(wordsContent[wordsContent.indexOf("number") + 1]);
      log.leg = Number(wordsContent[wordsContent.indexOf("leg") + 1]);
      return log;
    });
    setLogs(logs);
    return logs;
  }
  let [fullLogs, isLoading, errorMessege] = UseFetch("/Logs", preProccess);
  function onSearch(minTime, maxTime, orderBy, text) {
    OnSearch(orderBy, text, fullLogs, setLogs, (log) => {
      let contactDateTime = new Date(log.createdAt);
      return contactDateTime <= maxTime && contactDateTime >= minTime;
    });
  }
  let fields = [];
  if (logs && logs.length) {
    fields = Object.keys(logs[0]).filter(
      (fieldName) => fieldName.toLocaleLowerCase() !== "id"
    );
  }
  let TableHeaders = [
    "Content",
    "Date & Time",
    "Flight Number",
    "Airport Section",
  ];
  let options = TableHeaders.map((h) => {
    return { value: fields[TableHeaders.indexOf(h)], label: h };
  }).filter((o) => o.label !== "Content");
  return (
    <div className="View">
      <Rows>
        <SearchMenu
          options={options}
          onButtonSearchClick={onSearch}
        ></SearchMenu>
        {!errorMessege && !isLoading && (
          <Table fields={fields} dataMembers={logs} headers={TableHeaders} />
        )}
        {isLoading && <h4>Data is loading...</h4>}
        {errorMessege && <h4 style={{ color: "red" }}>{errorMessege}</h4>}
      </Rows>
    </div>
  );
};
