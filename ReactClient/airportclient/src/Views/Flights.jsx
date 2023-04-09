import { SearchMenu } from "../UIkit/Elements/SearchMenu/SerchMenu";
import { Table } from "../UIkit/Elements/Table/Table";
import { Rows } from "../UIkit/Layouts/Line/Line";
import "./Views.css";
import React, { useState } from "react";
import { UseFetch } from "../CustomHooks/FetchEffect";
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
    // console.log(minTime, maxTime, orderBy, text);

    let textFields = Object.keys(Fullflights[0]).filter(
      (field) => typeof Fullflights[0][field] ==='string'
    );
    let searchWords = text.toLocaleLowerCase().trim().split(" ");
    let filteredFlights = Fullflights.filter((flight) => {
      let contactDateTime = new Date(flight.madeContactAt);
      if (contactDateTime > maxTime || contactDateTime < minTime) {
        return false;
      }
      let flightString = "";
      textFields.forEach(field => {
        flightString += ` ${flight[field].toLocaleLowerCase()}`;
      });
      console.log(flightString);
      let hasSearchWords = true;
      searchWords.forEach(word => {
        if(!flightString.includes(word)){
          hasSearchWords = false;
        }
      });
      return hasSearchWords;
    });
    if(orderBy){
      let sortField = orderBy.value;
    filteredFlights.sort((f1,f2) =>(f1[sortField] > f2[sortField]) ? 1 : ((f2[sortField] > f1[sortField]) ? -1 : 0))
    }
    setFlights(filteredFlights);
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
  let options = TableHeaders.map(h =>{
    return {value:fields[TableHeaders.indexOf(h)], label:h}
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
