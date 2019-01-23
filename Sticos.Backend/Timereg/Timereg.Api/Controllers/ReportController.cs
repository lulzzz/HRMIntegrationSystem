using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using Timereg.Api.Domain.Interfaces;
using contracts = Timereg.Api.Contracts;
using domain = Timereg.Api.Domain.Models;

namespace Timereg.Api.Controllers
{
    [ApiController]
    [Route("reports")]
    public class ReportController : Controller
    {
        private readonly ITimeRegService _timeRegService;
        private readonly IMapper _mapper;

        public ReportController(ITimeRegService timeRegService, IMapper mapper)
        {
            _timeRegService = timeRegService;
            _mapper = mapper;
        }

        [HttpGet]
        [SwaggerOperation("Get")]
        public async Task<ActionResult<contracts.HourBalance>> GetHourBalance(int unitId, int employeeId)
        {
            var hourBalanceResult = await _timeRegService.GetHourBalance(unitId,employeeId);
            var mapHourBalance = _mapper.Map<domain.HourBalance, contracts.HourBalance>(hourBalanceResult);
            return Ok(mapHourBalance);
        }
    }
}
