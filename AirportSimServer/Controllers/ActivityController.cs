using AirportSimCore.Models.Dtos;
using AirportSimServer.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AirportSimServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityController : ControllerBase
    {
        private IAirportMonitorService _monitorService;
        private ILogger<ActivityController> _logger;
        public ActivityController(IAirportMonitorService MonitorService, ILogger<ActivityController> logger)
        {
            _monitorService = MonitorService;
            _logger = logger;

        }

        [HttpGet]
        public List<TerminalLegDto> Get()
        {
            _logger.LogInformation("Leg Status Requested !");
            return _monitorService.GetTerminalStatus();
        } 
    }
}
