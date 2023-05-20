
<img align="center" alt="airport" width="700px" style="padding-right:10px;" src="https://vid.alarabiya.net/images/2022/11/29/20a3b7ea-1f3e-408f-beca-d2108a5d6d23/20a3b7ea-1f3e-408f-beca-d2108a5d6d23.jpg?crop=1:1&width=1000" />  

# AirportSim
3 Projects in one - a simulator that sends flight requests (.net core console app), a client written in react and a web-api (asp.net) server holding all the logic and dealing with the requests
the simulator (console) generates and sends 'flights' to the api via http requests. The react UI fetches the data from the said api via http requests and gets updated with signalr.
The client can display previous flights and logs of activity in the airport, as well as current activity.
In this project I used DI and the Chain of responsibility pattern, and followed the SOLID design principles.

### Dependencies

* npm
* .net core
* SQL server
* signalR (react - client, nuget in server)

### Installing

* Run npm i in the client folder
* Add migration and update the local database

### Executing program

* Run the flight generator
* Run the web api
* Run the react clent
