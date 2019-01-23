using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using AutoMapper;

using Timereg.Api.Domain.Interfaces;
using Timereg.Api.Domain.Models;
using Shared.Contracts;
using contracts = Timereg.Api.Contracts;

namespace Timereg.Api.Controllers
{
    [ApiController]
    [Route("externalsystems")]
    public class ExternalSystemController : Controller
    {
        private readonly IExternalSystemService _externalSystemService;
        private readonly IMapper _mapper;

        public ExternalSystemController(IExternalSystemService externalSystemService, IMapper mapper)
        {
            _externalSystemService = externalSystemService;
            _mapper = mapper;
        }

        // GET: ExternalSystems
        [HttpGet]
        [SwaggerOperation(nameof(Search))]
        public async Task<ActionResult<IEnumerable<ICode>>> Search([FromQuery] contracts.SearchQueryExternalSystem query)
        {
            var mapQuery = _mapper.Map<contracts.SearchQueryExternalSystem, SearchQueryExternalSystem>(query);

            var queryResults = await _externalSystemService.Search(mapQuery);
            return Ok(queryResults);
        }

        [HttpGet("{id}/externaldata")]
        [SwaggerOperation(nameof(GetExternalData))]
        public async Task<ActionResult<IEnumerable<ExternalData>>> GetExternalData(int id, int unitId, int entity)
        {
            var queryResults = await _externalSystemService.GetExternalData(id, unitId, entity);
            return Ok(queryResults);
        }

        [HttpGet("{id}/matchentities")]
        [SwaggerOperation(nameof(MatchEntities))]
        public async Task<ActionResult<IEnumerable<EntityMatch>>> MatchEntities(int id, int unitId, int entity, [FromQuery(Name = "ids")]int[] ids)
        {
            var queryResult = await _externalSystemService.MatchEntities(id,unitId,entity,ids);
            return Ok(queryResult);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(nameof(GetExternalSystem))]
        public async Task<ActionResult<Timereg.Api.Contracts.ExternalSystem>> GetExternalSystem(int id)
        {
            var queryResult = await _externalSystemService.GetExternalSystem(id);
            var mapQuery = _mapper.Map<ExternalSystem, Timereg.Api.Contracts.ExternalSystem>(queryResult);
            return Ok(mapQuery);
        }
    }
}