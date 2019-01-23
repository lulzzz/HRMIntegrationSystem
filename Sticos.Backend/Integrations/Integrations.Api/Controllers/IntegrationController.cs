using AutoMapper;
using Integrations.Api.Contracts;
using Integrations.Api.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using contracts = Integrations.Api.Contracts;
using domain = Integrations.Api.Domain.Models;

namespace Integrations.Api.Controllers
{
    [ApiController]
    [Route("integrations")]
    public class IntegrationController : Controller
    {
        private readonly IIntegrationService _integrationService;
        private readonly IMapper _mapper;


        public IntegrationController(
            IIntegrationService integrationService,
            IMapper mapper
        )
        {
            _integrationService = integrationService;
            _mapper = mapper;
        }

        // GET: Integrations
        [HttpGet]
        [SwaggerOperation(nameof(Search))]
        public async Task<ActionResult<IEnumerable<contracts.Integration>>> Search([FromQuery]SearchQueryIntegration query)
        {
            var mapIntegration = _mapper.Map<SearchQueryIntegration, domain.SearchQueryIntegration>(query);

            var integrationsResult = await _integrationService.Search(mapIntegration);
            var integrationResultContracts = _mapper.Map<IEnumerable<domain.Integration>, IEnumerable<contracts.Integration>>(integrationsResult);
            return Ok(integrationResultContracts);
        }

        // GET: Integration/5
        [HttpGet("{id}")]
        [SwaggerOperation(nameof(Get))]
        public async Task<ActionResult<contracts.Integration>> Get(int id)
        {
            var integrationResult = await _integrationService.GetIntegration(id);
            var integrationResultContracts = _mapper.Map<domain.Integration, contracts.Integration>(integrationResult);

            return Ok(integrationResultContracts);
        }

        // POST
        [HttpPost]
        [SwaggerOperation(nameof(Create))]
        public async Task<ActionResult<contracts.Integration>> Create([FromBody] contracts.Integration integration)
        {
            var mapIntegration = _mapper.Map<contracts.Integration, domain.Integration>(integration);
            var integrationResult = await _integrationService.CreateIntegration(mapIntegration);

            var createdIntegrationContracts = _mapper.Map<domain.Integration, contracts.Integration>(integrationResult);

            return Ok(createdIntegrationContracts);
        }

        // PUT
        [HttpPut]
        [SwaggerOperation(nameof(Update))]
        public async Task<ActionResult<contracts.Integration>> Update([FromBody] contracts.Integration integration)
        {
            var mapIntegration = _mapper.Map<contracts.Integration, domain.Integration>(integration);
            var integrationResult = await _integrationService.UpdateIntegration(mapIntegration);

            var updatedIntegrationContracts = _mapper.Map<domain.Integration, contracts.Integration>(integrationResult);

            return Ok(updatedIntegrationContracts);
        }

        // DELETE
        [HttpDelete]
        [SwaggerOperation(nameof(Delete))]
        [SwaggerResponse((int)HttpStatusCode.NoContent)]
        public async Task<ActionResult> Delete([FromQuery] int id)
        {
            await _integrationService.Delete(id);
            return NoContent();
        }
    }
}