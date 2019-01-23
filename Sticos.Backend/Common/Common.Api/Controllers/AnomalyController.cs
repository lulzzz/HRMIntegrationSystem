using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Common.Api.Contracts;
using Common.Api.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Common.Api.Controllers
{
    [Route("anomalies")]
    public class AnomalyController : Controller
    {
        private readonly IAnomalyService _anomalyService;
        private readonly IMapper _mapper;

        public AnomalyController(IAnomalyService anomalyService, IMapper mapper)
        {
            _anomalyService = anomalyService;
            _mapper = mapper;
        }

        [HttpGet]
        [SwaggerOperation("Search")]
        public async Task<ActionResult<IEnumerable<Anomaly>>> Search([FromQuery] SearchQueryAnomaly query)
        {
            var mapQuery = _mapper.Map<SearchQueryAnomaly, Domain.Entities.SearchQueryAnomaly>(query);
            var queryResults = await _anomalyService.SearchAnomaly(mapQuery);

            var queryResultsContract = _mapper.Map<IEnumerable<Anomaly>>(queryResults);

            return Ok(queryResultsContract);
        }
    }
}