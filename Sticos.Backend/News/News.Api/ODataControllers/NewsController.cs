using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using News.Api.Middleware;
using News.Api.Repository;
using Shared.Interfaces;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace News.Api.ODataControllers
{
    [ODataRoutePrefix("news")]
    public class NewsController : ODataController
    {
        #region Fields
        private readonly ILogger<NewsController> _logger;
        private readonly IEntityService<Models.News, int, NewsContext> _entityService;
        #endregion

        #region Constructor
        public NewsController(
            ILogger<NewsController> logger,
            IEntityService<Models.News, int, NewsContext> entityService
            )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _entityService = entityService ?? throw new ArgumentNullException(nameof(entityService));
        }
        #endregion

        #region IInterface
        [EnableQuery]
        [ODataRoute("({key})")]
        [SwaggerOperation("GetById")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Models.News))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetById([FromODataUri] int key)
        {
            if (key <= 0) return BadRequest();

            var entity = await _entityService.GetById(key);
            if (entity != null)
            {
                return Ok(entity);
            }

            return NotFound();
        }

        [EnableQuery]
        [ODataRoute("({key})/attachments")]
        [SwaggerOperation(nameof(GetAttachments))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ICollection<Models.NewsAttachment>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAttachments([FromODataUri] int key)
        {
            if (key <= 0) return BadRequest();

            var entity = await _entityService.GetById(key);

            if (entity == null) return NotFound();

            var list = entity.Attachments;

            return Ok(list);
        }

        [EnableQuery]
        [ODataRoute("")]
        [SwaggerOperation("Search")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IQueryable<Models.News>))]
        public async Task<IActionResult> Get()
        {
            return Ok(await _entityService.GetQuery());
        }

        [EnableQuery]
        [ODataRoute("({key})")]
        [SwaggerOperation(nameof(Patch))]
        [AcceptVerbs("PATCH", "MERGE")]
        [ProducesResponseType((int)HttpStatusCode.Accepted, Type = typeof(Models.News))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Patch([FromODataUri] int key, Delta<Models.News> model)
        {
            if (model == null) return BadRequest();
            if (key <= 0) return BadRequest();

            var existingEntity = await _entityService.GetById(key);
            if (existingEntity == null)
            {
                _logger.LogError($"Entity not found: {key}");
                return NotFound();
            }

            model.Patch(existingEntity);

            var updatedEntity = await _entityService.Update(existingEntity);

            return Updated(updatedEntity);
        }

        [ValidateModel]
        [ODataRoute("({key})")]
        [HttpPut]
        [SwaggerOperation(nameof(Put))]
        [ProducesResponseType((int)HttpStatusCode.Accepted, Type = typeof(Models.News))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Put([FromODataUri] int key, Models.News model)
        {
            if (model == null) return BadRequest();
            if (key <= 0) return BadRequest();

            var updatedEntity = await _entityService.Update(model);

            return Updated(updatedEntity);
        }


        [ValidateModel]
        [ODataRoute("")]
        [HttpPost]
        [SwaggerOperation(nameof(Post))]
        [ProducesResponseType((int)HttpStatusCode.Created, Type = typeof(Models.News))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Post(Models.News model)
        {
            if (model == null) return BadRequest();

            var createdEntity = await _entityService.Create(model);

            return Created(createdEntity);
        }

        [HttpDelete]
        [ODataRoute("({key})")]
        [SwaggerOperation(nameof(Delete))]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Delete([FromODataUri] int key)
        {
            if (key <= 0) return BadRequest();

            var isDeleted = await _entityService.Delete(key);
            if (!isDeleted)
            {
                var msg = $"Unable to delete News with id {key}";
                _logger.Log(LogLevel.Warning, msg);
                throw new Exception(msg);
            }

            return Ok();
        }
        #endregion

        #region Methods
        #endregion
    }
}
