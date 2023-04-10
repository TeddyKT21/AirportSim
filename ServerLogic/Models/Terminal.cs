
using AirportSimCore.Interfaces;
using AirportSimCore.Models;
using AirportSimCore.Models.Dtos;
using AirportSimCore.Interfaces;
using Microsoft.Extensions.Logging;
using ServerDal.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLogic.Models
{
    public class Terminal : ITerminal
    {
        private List<TerminalLeg> _legs;

        public List<TerminalLeg> Legs { get { return _legs; } }

        private Queue<Flight> _arrivingFlights;
        
        private Queue<Flight> _departingFlights;

        private List<TerminalLeg> _recivingDepartureLegs;

        private List<TerminalLeg> _recivingArrivalLegs;

        private readonly ILegServiceFactory _serviceFactory;
        private readonly IAirportRepository _repository;

        public Terminal(ILegServiceFactory serviceFactory, IAirportRepository airportRepository) 
        {
            _serviceFactory = serviceFactory;
            _repository = airportRepository;
            _recivingDepartureLegs = new();
            _recivingArrivalLegs = new();
            _departingFlights = new();
            _arrivingFlights = new();
            _legs = new();
            if (_repository.FetchAll<TerminalLegDto>() == null || !_repository.FetchAll<TerminalLegDto>().Any())
            {
                setUpDefaultLegs();
            }
            ConstructLegs();
            foreach (var leg in _recivingArrivalLegs)
            {
                leg.OnEmptiedLeg += StartArrivingProccess;
            }
            foreach (var leg in _recivingDepartureLegs)
            {
                leg.OnEmptiedLeg += StartDepartingProccess;
            }
        }

        private void setUpDefaultLegs()
        {
            var legOne = new TerminalLegDto() { LegNumber = 1, Duration = 5, IsArrivingStart = true};
            var legTwo = new TerminalLegDto() { LegNumber = 2, Duration = 5};
            var legThree = new TerminalLegDto() { LegNumber = 3, Duration = 5 };
            var legFour = new TerminalLegDto() { LegNumber = 4, Duration = 15 };
            var legFive = new TerminalLegDto() { LegNumber = 5, Duration = 10 };
            var legSix = new TerminalLegDto() { LegNumber = 6, Duration = 20, IsDepartingStart = true };
            var legSeven = new TerminalLegDto() { LegNumber = 7, Duration = 20, IsDepartingStart = true };
            var legEight = new TerminalLegDto() { LegNumber = 8, Duration = 10 };

            _repository.Add(legOne);
            _repository.Add(legTwo);
            _repository.Add(legThree);
            _repository.Add(legFour);
            _repository.Add(legFive);
            _repository.Add(legSix);
            _repository.Add(legSeven);
            _repository.Add(legEight);

            legOne.NextLegConnections = new List<TerminalLegConnection>() {new TerminalLegConnection() { TerminalLegStart = legOne, TerminalLegContinue = legTwo, IsDepartingConnection = false } };
            legTwo.NextLegConnections = new List<TerminalLegConnection>() { new TerminalLegConnection() { TerminalLegStart = legTwo, TerminalLegContinue = legThree, IsDepartingConnection = false } };
            legThree.NextLegConnections = new List<TerminalLegConnection>() { new TerminalLegConnection() { TerminalLegStart = legThree, TerminalLegContinue = legFour, IsDepartingConnection = false } };
            legFour.NextLegConnections = new List<TerminalLegConnection>() { new TerminalLegConnection() { TerminalLegStart = legFour, TerminalLegContinue = legFive, IsDepartingConnection = false } };
            legFive.NextLegConnections = new List<TerminalLegConnection>() { new TerminalLegConnection() { TerminalLegStart = legFive, TerminalLegContinue = legSix, IsDepartingConnection = false },
                                                                             new TerminalLegConnection() { TerminalLegStart = legFive, TerminalLegContinue = legSeven, IsDepartingConnection = false } };
            legSix.NextLegConnections = new List<TerminalLegConnection>() { new TerminalLegConnection() { TerminalLegStart = legSix, TerminalLegContinue = legEight, IsDepartingConnection = true } };
            legSeven.NextLegConnections = new List<TerminalLegConnection>() { new TerminalLegConnection() { TerminalLegStart = legSeven, TerminalLegContinue = legEight, IsDepartingConnection = true } };
            legEight.NextLegConnections = new List<TerminalLegConnection>() { new TerminalLegConnection() { TerminalLegStart = legEight, TerminalLegContinue = legFour, IsDepartingConnection = true } };

            _repository.Update(legOne);
            _repository.Update(legTwo);
            _repository.Update(legThree);
            _repository.Update(legFour);
            _repository.Update(legFive);
            _repository.Update(legSix);
            _repository.Update(legSeven);
            _repository.Update(legEight);
          

        }

        private void ConstructLegs()
        {
            _legs = new List<TerminalLeg>();
            var legDtos = _repository.FetchAllLegs();
            foreach (var legDto in legDtos)
            {
                var flightLogCreator = _serviceFactory.GetFlightLogCreator();
                var hub = _serviceFactory.GetSignalR();
                var logger = _serviceFactory.GetLogger<TerminalLeg>();
                var leg = new TerminalLeg(legDto.LegNumber, legDto.Duration, flightLogCreator, hub, logger);
                _legs.Add(leg);
                if (legDto.IsDepartingStart)
                {
                    _recivingDepartureLegs.Add(leg);
                }
                if (legDto.IsArrivingStart)
                {
                    _recivingArrivalLegs.Add(leg);
                }
            }
            foreach (var legDto in legDtos)
            {
                var leg = _legs.First(l => l.LegNumber == legDto.LegNumber);
                foreach (var connection in legDto.NextLegConnections!)
                {
                    var nextLeg = _legs.First(l => l.LegNumber == connection!.TerminalLegContinue!.LegNumber);
                    if(nextLeg != null && connection.IsDepartingConnection)
                    {
                        leg.AddLegToDeparting(nextLeg);
                    }
                    else if(nextLeg != null) 
                    {
                        leg.AddLegToArriving(nextLeg);
                    }
                }
            }
        }

        public async Task AddFlight(Flight flight)
        {
            await _repository.AddAsync(flight);
            _ = Task.Run(() =>
            {
                if (flight == null)
                {
                    throw new ArgumentNullException("flight cannot be null !");
                }
                var recivingLegs = _recivingArrivalLegs;
                if (flight.IsDeparting)
                {
                    recivingLegs = _recivingDepartureLegs;
                }
                bool foundEmptyLeg = false;
                foreach (TerminalLeg leg in recivingLegs)
                {
                    lock(leg)
                    {
                        if (leg.CurrentFlight == null)
                        {
                            foundEmptyLeg = true;
                            leg.CurrentFlight = flight;
                            break;
                        }
                    }
                }
                if (!foundEmptyLeg)
                {
                    var flightQueue = _arrivingFlights;
                    if (flight.IsDeparting)
                    {
                        flightQueue = _departingFlights;
                    }
                    flightQueue.Enqueue(flight);
                };
            });
            
        }

        private void StartDepartingProccess(TerminalLeg freedUpLeg)
        {
            lock (freedUpLeg)
            {
                if (_departingFlights.Count > 0 && freedUpLeg.CurrentFlight == null)
                {
                    var departingFlight = _departingFlights.Dequeue();
                    freedUpLeg.CurrentFlight = departingFlight;
                }
            }
        }

        private void StartArrivingProccess(TerminalLeg freedUpLeg)
        {
            lock (freedUpLeg)
            {
                if (_arrivingFlights.Count > 0 && freedUpLeg.CurrentFlight != null)
                {
                    var arrivingFlight = _arrivingFlights.Dequeue();
                    freedUpLeg.CurrentFlight = arrivingFlight;
                }
            }
        }
    }
}
