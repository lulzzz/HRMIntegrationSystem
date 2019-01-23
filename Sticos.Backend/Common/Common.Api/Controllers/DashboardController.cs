using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Common.Api.Contracts;
using Common.Api.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.Interfaces;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Common.Api.Controllers
{
    [ApiController]
    [Route("dashboards")]
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;
        private readonly IMapper _mapper;
        private readonly ICurrentUserContext _currentUserContext;
        private readonly IOwnerTypeService _ownerTypeService;

        public DashboardController(IDashboardService dashboardService, IMapper mapper, ICurrentUserContext currentUserContext, IOwnerTypeService ownerTypeService)
        {
            _dashboardService = dashboardService;
            _mapper = mapper;
            _currentUserContext = currentUserContext;
            _ownerTypeService = ownerTypeService;
        }

        [HttpPost]
        [SwaggerOperation("Create")]
        [SwaggerResponse((int)HttpStatusCode.Created, typeof(Dashboard))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<Dashboard>> CreateDashboard([FromBody] Dashboard dashboardResource)
        {
            dashboardResource = await SetOwnerTypeDefault(dashboardResource);
            
            var mapDashboard = _mapper.Map<Dashboard, Domain.Entities.Dashboard>(dashboardResource);
            var createdDashboard = await _dashboardService.Create(mapDashboard);

            var createdDashboardContract = _mapper.Map<Dashboard>(createdDashboard);
            return CreatedAtAction(nameof(GetDashboard),
                new { Controller = "Dashboard", id = createdDashboardContract.Id }, createdDashboardContract);
        }

        [HttpPut]
        [SwaggerOperation("Update")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(Dashboard))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<Dashboard>> UpdateDashboard([FromBody] Dashboard dashboardResource)
        {
            var mapDashboard = _mapper.Map<Dashboard, Domain.Entities.Dashboard>(dashboardResource);
            var updatedDashboard = await _dashboardService.Update(mapDashboard);

            var updateDashobardContract = _mapper.Map<Dashboard>(updatedDashboard);
            return Ok(updateDashobardContract);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation("Delete")]
        [SwaggerResponse((int)HttpStatusCode.NoContent)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteDashboard(int id)
        {
            await _dashboardService.Delete(id);

            return NoContent();
        }

        [HttpGet("{id}")]
        [SwaggerOperation("Get")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(Dashboard))]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<Dashboard>> GetDashboard(int id)
        {
            var dashboard = await _dashboardService.GetById(id);
            var mapDashboard = _mapper.Map<Domain.Entities.Dashboard, Dashboard>(dashboard);
            return Ok(mapDashboard);
        }

        [HttpGet]
        [SwaggerOperation("Search")]
        public async Task<ActionResult<ICollection<Dashboard>>> Search([FromQuery] SearchQueryDashboard query)
        {
            var mapQuery = _mapper.Map<SearchQueryDashboard, Domain.Entities.SearchQueryDashboard>(query);
            var userContext = _currentUserContext.Get();
            mapQuery.UserId = userContext.UserId;
            var queryResults = await _dashboardService.Search(mapQuery);
            var queryResultsContract = _mapper.Map<ICollection<Dashboard>>(queryResults);
            return Ok(queryResultsContract);
        }

        private async Task<Dashboard> SetOwnerTypeDefault(Dashboard dashboardResource)
        {
            if (!dashboardResource.OwnerTypeId.HasValue)
            {
                var ownerTypeResult = await _ownerTypeService.Search(new Domain.Entities.SearchQueryOwnerType
                {
                    Name = "User",
                    Take = 1
                });
                var userOwnerType = ownerTypeResult.First();
                dashboardResource.OwnerTypeId = userOwnerType.Id;

                var userContext = _currentUserContext.Get();
                dashboardResource.OwnerId = userContext.UserId;
            }

            return dashboardResource;
        }
    }
}