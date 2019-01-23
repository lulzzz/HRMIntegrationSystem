using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using AutoMapper;

using Altinn.Api.Contratcs;
using Altinn.Api.Domain.Interfaces;

namespace Altinn.Api.Controllers
{
    [Route("navmessages")]
    public class NavMessagesController : Controller
    {
        private readonly INavMessageService _messageService;
        private readonly IMapper _mapper;
        
        public NavMessagesController(INavMessageService messageService, IMapper mapper)
        {
            _messageService = messageService;
            _mapper = mapper;
        }

        [HttpGet]
        [SwaggerOperation("Search")]
        public async Task<ActionResult<IEnumerable<NavMessage>>> Search([FromQuery] SearchQueryNavMessage query)
        {
            var mapQuery = _mapper.Map<SearchQueryNavMessage, Domain.Entities.SearchQueryNavMessage>(query);
            var queryResults = await _messageService.Search(mapQuery);

            return Ok(queryResults);
        }
    }
}