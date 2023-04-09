using AirportSimCore.Interfaces;
using AirportSimCore.Models;
using Microsoft.AspNetCore.Mvc;
using NLog.Fluent;
using ServerDal.Repositories;
using ServerLogic.Models;
using System;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AirportSimServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private ITerminal _terminal;
        private readonly ILogger _logger;
        private IAirportRepository _repository;
        public FlightsController(IAirportRepository repository, ITerminal terminal, ILogger<FlightsController> logger) 
        {
            _repository = repository;
            _terminal = terminal;
            _logger = logger;
        }
        [HttpGet]
        public List<Flight> GetFlights()
        {
            List<Flight> flights= _repository.FetchAll<Flight>().ToList();
            _logger.LogInformation($"Server got a request for flights at {DateTime.Now:F}");
            return flights;
        }

       

        [HttpPost]
        public async Task<IActionResult> AddFlight(Flight flightDto)
        {
            _logger.LogInformation($"Flight number {flightDto.FlightNumber} made first contact with the Airport at {flightDto.MadeContactAt}");
            try
            {
                await _terminal.AddFlight(flightDto);
                _logger.LogInformation($"Flight number {flightDto.FlightNumber} sucssesfully registered at the airport at {DateTime.Now}");
            }
            catch (Exception e)
            {

                _logger.LogError($"an error ocured !!! {e.Message}");
                return StatusCode(500);
            }
            return StatusCode(201, Ok());
        }
    }
    
}
