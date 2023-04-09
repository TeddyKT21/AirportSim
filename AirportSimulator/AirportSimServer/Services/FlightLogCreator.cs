using AirportSimCore.Interfaces;
using AirportSimCore.Models;
using AirportSimCore.Models.Dtos;
using ServerDal.Repositories;
using ServerLogic.Models;

namespace AirportSimServer.Services
{
    public class FlightLogCreator : IFlightLogCreator
    {
        private IAirportRepository _repository; 
        public FlightLogCreator(IAirportRepository repository, ILogger<FlightLogCreator> logger) 
        {
            _repository = repository;
        }
        public void CreateLogWhenEntering(int legNumber, int flightId)
        {
             CreateLog(legNumber, flightId, true);
        }

        public void CreateLogWhenExiting(int legNumber, int flightId)
        {
             CreateLog(legNumber, flightId, false);
        }


        private  void CreateLog(int legNumber, int flightId, bool isEntering)
        {
            Task.Run(async () =>
            {
                string action = "Left";
                if (isEntering)
                {
                    action = "Entered";
                }
                Flight flight = await _repository.FindByIdAsync<Flight>(flightId);
                TerminalLegDto LegDto = await _repository.FindFirstAsync<TerminalLegDto>(l => l.LegNumber == legNumber);
                string text = $"Flight number {flight.FlightNumber} {action} leg {legNumber} at {DateTime.Now}";
                LegLogDto log = new LegLogDto()
                {
                    CreatedAt = DateTime.Now,
                    Flight = flight,
                    Leg = LegDto,
                    Text = text,

                };
                await _repository.AddAsync(log);
            });
        }
    }
}
