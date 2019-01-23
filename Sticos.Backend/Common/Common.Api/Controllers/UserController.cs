using AutoMapper;
using Common.Api.Contracts.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using domain = Common.Api.Domain.Interfaces;

namespace Common.Api.Controllers
{
    [Route("users")]
    public class UserController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;
        private readonly domain.IUserService _userService;

        public UserController(domain.IUserService userService, IMapper mapper, ILogger<UserController> logger)
        {
            _userService = userService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        [SwaggerOperation("Search")]
        public async Task<ActionResult<ICollection<IUser>>> Search([FromQuery] SearchQueryUser query)
        {
            var mapQuery = _mapper.Map<SearchQueryUser, Domain.Entities.SearchQueryUser>(query);
            var queryResults = await _userService.SearchUser(mapQuery);

            return Ok(queryResults);
        }

        [HttpGet("{userId}")]
        [SwaggerOperation("GetUser")]
        public async Task<ActionResult<IUser>> GetUser(int userId)
        {
            var user = await _userService.GetById(userId);
            return Ok(user);
        }
    }
}


