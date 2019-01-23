using AutoMapper;
using Common.Api.Contracts.Employees;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Services.Constants;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Threading.Tasks;
using domain = Common.Api.Domain.Interfaces;

namespace Common.Api.Controllers
{
    [Route("employees")]
    public class EmployeeController : Controller
    {
        private readonly domain.IEmployeeService _employeeService;
        private readonly IMapper _mapper;

        public EmployeeController(domain.IEmployeeService employeeService, IMapper mapper)
        {
            _employeeService = employeeService;
            _mapper = mapper;
        }

        [HttpGet]
        [SwaggerOperation("Search")]
        [Authorize(Policy = AuthConstants.CustomerAdminPolicyName)]
        public async Task<ActionResult<IEnumerable<IEmployee>>> Search([FromQuery] SearchQueryEmployee query)
        {
            var domainEntity = _mapper.Map<Common.Api.Domain.Entities.SearchQueryEmployee>(query);
            var queryResults = await _employeeService.SearchEmployee(domainEntity);
            return Ok(queryResults);
        }
    }
}