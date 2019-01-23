using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using domain = Common.Api.Domain.Entities;
using contracts = Common.Api.Contracts;
using System.Linq;
using Common.Api.Contracts;
using Common.Api.Domain.Interfaces;
using Shared.Interfaces.Queries;
using sharedDomain = Shared.Domain.ValueObjects.Queries;

namespace Common.Api.Controllers
{
    [Route("units")]
    public class UnitController : Controller
    {
        private readonly IUnitService _unitService;
        private readonly IUnitQueries _unitQueries;
        private readonly IMapper _mapper;

        public UnitController(IMapper mapper, IUnitService unitService, IUnitQueries unitQueries)
        {
            _unitService = unitService;
            _unitQueries = unitQueries;
            _mapper = mapper;
        }

        [HttpGet]
        [SwaggerOperation("GetUnits")]
        public async Task<ActionResult<IEnumerable<Unit>>> Search([FromQuery] SearchQueryUnit query)
        {
            var domainEntity = _mapper.Map<Common.Api.Domain.Entities.SearchQueryUnit>(query);

            var unitsResult = await _unitService.Search(domainEntity);
            var unitsResultContracts = _mapper.Map<IEnumerable<domain.Unit>, IEnumerable<contracts.Unit>>(unitsResult);

            return Ok(unitsResultContracts);
        } 

        [HttpGet("{id}")]
        [SwaggerOperation("GetUnit")]
        public async Task<ActionResult<Unit>> GetUnit(int id)
        {
            var unitResult = await _unitService.GetUnit(id);
            var unitResultContract = _mapper.Map<domain.Unit, contracts.Unit> (unitResult);

            return Ok(unitResultContract);
        }     
       
        [HttpGet("{id}/HierarchyUp")]
        [SwaggerOperation("GetHierarchyUp")]
        public async Task<ActionResult<IEnumerable<contracts.UnitWithParent>>> GetHierarchyUp(int id)
        {
            var unitResult = await _unitQueries.GetHierarchyUp(id);
            var unitResultContract = _mapper.Map<IEnumerable<sharedDomain.UnitWithParent>, IEnumerable<contracts.UnitWithParent>> (unitResult);

            return Ok(unitResultContract);
        }

        [HttpGet("{id}/HierarchyDown")]
        [SwaggerOperation("GetHierarchyDown")]
        public async Task<ActionResult<IEnumerable<contracts.UnitWithParent>>> GetHierarchyDown(int id)
        {
            var unitResult = await _unitQueries.GetHierarchyDown(id);
            var unitResultContract = _mapper.Map<IEnumerable<sharedDomain.UnitWithParent>, IEnumerable<contracts.UnitWithParent>> (unitResult);

            return Ok(unitResultContract);
        }
    }
}