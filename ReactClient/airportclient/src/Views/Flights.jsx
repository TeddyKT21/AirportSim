import { SearchMenu } from "../UIkit/Elements/SearchMenu/SerchMenu";
import { Table } from "../UIkit/Elements/Table/Table";
import { Rows } from "../UIkit/Layouts/Line/Line";
import "./Views.css";
import React, { useState } from "react";
import { UseFetch } from "../CustomHooks/FetchEffect";
import { OnSearch } from "./OnSearch";
export const Flights = (props) => {
  let [flights, setFlights] = useState(null);
  function preProccess(flights) {
    if (!flights) {
      return;
    }
    flights = flights.map((f) => {
      f.isDeparting
        ? (f.isDeparting = "Departure")
        : (f.isDeparting = "Arrival");
      return f;
    });
    setFlights(flights);
    return flights;
  }
  let [Fullflights, isLoading, errorMessege] = UseFetch(
    "/Flights",
    preProccess
  );
  function onSearch(minTime, maxTime, orderBy, text) {
    OnSearch(orderBy, text, Fullflights, setFlights, (flight) => {
      let contactDateTime = new Date(flight.madeContactAt);
      return contactDateTime <= maxTime && contactDateTime >= minTime;
    });
  }
  let fields = [];
  if (flights && flights.length) {
    fields = Object.keys(flights[0]).filter(
      (fieldName) => fieldName.toLocaleLowerCase() !== "id"
    );
  }
  let TableHeaders = [
    "Flight number",
    "Passenger count",
    "Plane model",
    "Airline",
    "Departue/Arrival",
    "Time of contact",
  ];
  let options = TableHeaders.map((h) => {
    return { value: fields[TableHeaders.indexOf(h)], label: h };
  }).filter((o) => o.label !== "Departue/Arrival");
  return (
    <div className="View">
      <Rows>
        <SearchMenu
          options={options}
          onButtonSearchClick={onSearch}
        ></SearchMenu>
        {!errorMessege && !isLoading && (
          <Table fields={fields} dataMembers={flights} headers={TableHeaders} />
        )}
        {isLoading && <h4>Data is loading...</h4>}
        {errorMessege && <h4 style={{ color: "red" }}>{errorMessege}</h4>}
      </Rows>
    </div>
  );
};
