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
    [Route("ownertypes")]
    public class OwnerTypeController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IOwnerTypeService _ownerTypeService;

        public OwnerTypeController(IOwnerTypeService ownerTypeService, IMapper mapper)
        {
            _ownerTypeService = ownerTypeService;
            _mapper = mapper;
        }

        [HttpGet]
        [SwaggerOperation("Search")]
        public async Task<ActionResult<ICollection<OwnerType>>> Search([FromQuery] SearchQueryOwnerType query)
        {
            var mapQuery = _mapper.Map<SearchQueryOwnerType, Domain.Entities.SearchQueryOwnerType>(query);
            var queryResults = await _ownerTypeService.Search(mapQuery);

            var queryResultsContract = _mapper.Map<ICollection<OwnerType>>(queryResults);

            return Ok(queryResultsContract);
        }
    }
}