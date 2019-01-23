using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Common.Api.Contracts;
using Common.Api.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts;
using Swashbuckle.AspNetCore.SwaggerGen;
using domain = Common.Api.Domain.Entities;

namespace Common.Api.Controllers
{
    [ApiController]
    [Route("absencestypes")]
    public class AbsenceTypeController : Controller
    {
        private readonly IAbsenceTypeService _service;
        private readonly IMapper _mapper;

        public AbsenceTypeController(IAbsenceTypeService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        [SwaggerOperation("Get")]
        public async Task<ActionResult<IEnumerable<ICode>>> GetAbsenceTypes([FromQuery] SearchQueryAbsenceType query)
        {
            var mappedQuery = _mapper.Map<domain.SearchQueryAbsenceType>(query);
            var absenceTypes = await _service.GetAbsenceTypes(mappedQuery);
            return Ok(absenceTypes);
        }
    }
}