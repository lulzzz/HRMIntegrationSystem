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
    [Route("notifications")]
    public class NotificationController : Controller
    {
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService, IMapper mapper)
        {
            _notificationService = notificationService;
            _mapper = mapper;
        }

        [HttpGet]
        [SwaggerOperation("Search")]
        public async Task<ActionResult<ICollection<Notification>>> Search([FromQuery] SearchQueryNotification query)
        {
            var mapQuery = _mapper.Map<SearchQueryNotification, Domain.Entities.SearchQueryNotification>(query);
            var queryResults = await _notificationService.SearchNotification(mapQuery);

            var queryResultContract = _mapper.Map<ICollection<Notification>>(queryResults);

            return Ok(queryResultContract);
        }
    }
}