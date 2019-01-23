using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using AutoMapper;

using Altinn.Api.Domain.Interfaces;
using Altinn.Api.Domain.Entities;
using contracts = Altinn.Api.Contratcs;
using Shared.Contracts;

namespace Altinn.Api.Controllers
{
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
        public async Task<ActionResult<IEnumerable<ExternalData>>> GetExternalData(ExternalGovernmentSystem id)
        {
            var queryResult = await _externalSystemService.GetExternalData(id);
            return Ok(queryResult);
        }
    }
}