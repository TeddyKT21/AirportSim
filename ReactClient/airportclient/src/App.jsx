import { NavLink, Routes,Route } from 'react-router-dom';
import './App.css';
import {RowsNavMenu, SaparateMain } from './UIkit/Layouts/Line/Line';
import { CurrentActivity } from './Views/CurrentActivity';
import { FlightLogs } from './Views/FlightLogs';
import { Flights } from './Views/Flights';
import React from 'react';

function App() {
  return (
    <div className="App">
      <SaparateMain>
        <RowsNavMenu>
          <NavLink to='/Flights'>Registered Flights</NavLink>
          <NavLink to='/FlightLogs'>Flights Logs</NavLink>
          <NavLink to='/LiveActivity'>Current Airport Activity</NavLink>
        </RowsNavMenu>
        <div className='ContentWindow'>
          <Routes>
            <Route path='/Flights' element={<Flights/>}></Route>
            <Route path='/FlightLogs' element={<FlightLogs/>}></Route>
            <Route path='/LiveActivity' element={<CurrentActivity/>}></Route>
          </Routes>
        </div>
      </SaparateMain>
    </div>
  );
}

export default App;
