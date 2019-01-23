using System.Threading.Tasks;
using AutoMapper;
using Common.Api.Contracts;
using Common.Api.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Common.Api.Controllers
{
    [Route("brregentity")]
    public class BrregEntityController : Controller
    {
        private readonly IBrregService _brregService;
        private readonly IMapper _mapper;

        public BrregEntityController(IBrregService brregService, IMapper mapper)
        {
            _brregService = brregService;
            _mapper = mapper;
        }

        [HttpGet("{organizationNumber}")]
        public async Task<ActionResult<BrregEntity>> GetBrregEntity(int organizationNumber, [FromQuery] bool includeChildren)
        {
            var brregEntity = await _brregService.GetBrregEntity(organizationNumber, includeChildren);
            return _mapper.Map<BrregEntity>(brregEntity);
        }
    }
}