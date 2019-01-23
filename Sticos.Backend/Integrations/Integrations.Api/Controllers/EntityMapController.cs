using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using Integrations.Api.Contracts;
using Integrations.Api.Domain.Interfaces;
using domain = Integrations.Api.Domain.Models;

namespace Integrations.Api.Controllers
{
    [ApiController]
    [Route("entitymaps")]
    public class EntityMapController : Controller
    {

        private readonly IEntityMapService _entityMapService;
        private readonly IMapper _mapper;

        public EntityMapController(IEntityMapService entityMapService, IMapper mapper)
        {
            _entityMapService = entityMapService;
            _mapper = mapper;
        }

        // PUT
        [HttpPut]
        [SwaggerOperation("Update")]
        public async Task<ActionResult<IEnumerable<EntityMap>>> Update([FromBody] IEnumerable<EntityMap> entityMaps)
        {
            var mapEntity = _mapper.Map<IEnumerable<domain.EntityMap>>(entityMaps);
            var updatedEntityMapsResult = await _entityMapService.UpdateEntityMaps(mapEntity);

            var updatedEntityMapsContracts = _mapper.Map<IEnumerable<Domain.Models.EntityMap>, IEnumerable<EntityMap>>(updatedEntityMapsResult.ToList());
            return Ok(updatedEntityMapsContracts);
        }

        // Get
        [HttpGet]
        [SwaggerOperation("Get")]
        public async Task<ActionResult<IEnumerable<EntityMap>>> GetEntityMaps([FromQuery]SearchQueryEntityMap entityMaps)
        {
            var mapEntity = _mapper.Map<domain.SearchQueryEntityMap>(entityMaps);

            var entityMapsResult = await _entityMapService.Search(mapEntity);

            var entityMapsContracts = _mapper.Map<IEnumerable<Domain.Models.EntityMap>, IEnumerable<EntityMap>>(entityMapsResult.ToList());
            return Ok(entityMapsContracts);
        }
    }
}