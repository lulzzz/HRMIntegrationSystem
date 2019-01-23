using System.Threading.Tasks;
using Absence.Api.Domain.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Charts;
using Swashbuckle.AspNetCore.SwaggerGen;
namespace Absence.Api.Controllers
{
    [ApiController]
    [Route("statistics")]
    public class StatisticsController : Controller
    {
        private readonly IStatisticsService _service;
        private readonly IMapper _mapper;

        public StatisticsController(IStatisticsService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        [SwaggerOperation("Get")]
        public async Task<ActionResult<IChart>> GetStatistics(int id)
        {
            var domainChart = await _service.GetStatistics(id);
            var contractChart = _mapper.Map<IChart>(domainChart);
            return Ok(contractChart);
        }
    }
}
