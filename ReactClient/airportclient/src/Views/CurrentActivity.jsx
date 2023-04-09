import { Table } from "../UIkit/Elements/Table/Table";
import { Rows } from "../UIkit/Layouts/Line/Line";
import "./Views.css";
import React, { useEffect, useRef, useState } from "react";
import { UseFetch } from "../CustomHooks/FetchEffect";
import * as signalR from "@microsoft/signalr";
const baseUrl = require("../URL.json").url;
let isLoading = true;
let errorMessege = null;
export const CurrentActivity = (props) => {
  let [Sections, setSections] = useState(null);
  let firstRender = useRef(false);
  firstRender.current = false;
  function preProccess(Sections) {
    if (!Sections) {
      return null;
    }
    Sections = Sections.map((s) => {
      s.id = s.legNumber;
      if(s.currentFlight){
        if(s.currentFlight.isDeparting){
          s.currentFlight.isDeparting = "departing";
        }
        else {
          s.currentFlight.isDeparting = "arriving";
        }
        s = {...s,...s.currentFlight};
        if (s.isOperating) {
          s.Status = "operating";
        } else {
          s.Status = "pending";
        }
      }
      else{
        let flightFields = {
          flightNumber : "-",
          airLine : "-",
          isDeparting : "-",
          madeContactAt : "-",
          numberOfPassengers : '-',
          planeModel : '-',
          Status : '-'
        }
        s = {...s,...flightFields};
      }
      delete s.currentFlight;
      return s;
    });
    firstRender.current = true;
    setSections(Sections);
    return Sections;
  }
  [Sections, isLoading, errorMessege] = UseFetch("/Activity", preProccess);
  let fields = [];
  useEffect(() => {
    if(!Sections){
      return;
    }
    const connection = new signalR.HubConnectionBuilder()
      .withUrl(baseUrl.replace("/api", "/StamHub"), {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets,
      })
      .build();

    connection.start().catch((err) => console.error(err));
    connection.on("SectionUpdate", (messege) => {
      if (!Sections) {
        return;
      }
      if(messege.flight){
        if(messege.flight.isDeparting){
          messege.flight.isDeparting = "departing";
        }
        else {
          messege.flight.isDeparting = "arriving";
        }
      }
      let updatedSection = Sections.find(
        (s) => s.legNumber === messege.legNumber
      );
      if (messege.isEntering) {
        updatedSection.flightNumber = messege.flight.flightNumber;
        updatedSection.airLine = messege.flight.airLine;
        updatedSection.isDeparting = messege.flight.isDeparting;
        updatedSection.madeContactAt = messege.flight.madeContactAt;
        updatedSection.numberOfPassengers = messege.flight.numberOfPassengers;
        updatedSection.planeModel = messege.flight.planeModel;
        updatedSection.Status = "operating";
        updatedSection.isOperating = true;
      } else {
        updatedSection.flightNumber = "-";
        updatedSection.airLine = "-";
        updatedSection.isDeparting = "-";
        updatedSection.madeContactAt = "-";
        updatedSection.numberOfPassengers = '-';
        updatedSection.planeModel = '-';
        updatedSection.Status = "-";
        updatedSection.isOperating = false;
      }
      setSections({...Sections});
    });
    connection.on("SectionPendingUpdate", (legNumber) => {
      if (!Sections) {
        return;
      }
      let updatedSection = Sections.find((s) => s.legNumber === legNumber);
      updatedSection.Status = "pending";
      updatedSection.isOperating = false;
      setSections({...Sections});
    });
  }, [Sections && firstRender.current]);

  if (Sections) {
    fields = Object.keys(Sections[0]).filter(
      (fieldName) =>
        fieldName.toLocaleLowerCase() !== "id" &&
        fieldName.toLocaleLowerCase() !== "isarrivingstart" &&
        fieldName.toLocaleLowerCase() !== "isdepartingstart" &&
        fieldName.toLocaleLowerCase() !== "nextlegconnections" &&
        fieldName.toLocaleLowerCase() !== "duration" &&
        fieldName.toLocaleLowerCase() !== "isoperating"
    );
    if (!fields.find((s) => s === "Status")) {
      fields.splice(1,0,"Status");
    }
  }

  let TableHeaders = [
    "Section Number",
    "Flight Number",
    "Airline",
    "Departue/Arrival",
    "Time of contact",
    "Passenger Count",
    "Plane Model",
    'Section Status'
  ];
  return (
    <div className="View">
      <Rows>
        {!errorMessege && !isLoading && (
          <Table fields={fields} dataMembers={Sections} headers={TableHeaders}/>
        )}
        {isLoading && <h4>Data is loading...</h4>}
        {errorMessege && <h4 style={{ color: "red" }}>{errorMessege}</h4>}
      </Rows>
    </div>
  );
};
