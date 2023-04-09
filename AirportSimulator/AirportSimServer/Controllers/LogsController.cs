using AirportSimCore.Interfaces;
using AirportSimCore.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using ServerDal.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AirportSimServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IAirportRepository _repository;
        public LogsController(ILogger<LogsController> logger, IAirportRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }
        [HttpGet]
        public async Task<List<LegLogDto>> Get()
        {
            return (await _repository.FetchAllAsync<LegLogDto>()).ToList();
        }



        //[HttpPost]
        //public async Task<IEnumerable<LegLogDto>> Post([FromBody] DateTime start , [FromBody] DateTime end)
        //{
        //    return await _repository.FilterByAsync<LegLogDto>((l) => l.CreatedAt > start && l.CreatedAt < end);

        //}


    }
}
