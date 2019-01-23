using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using News.Api.Middleware;
using News.Api.Repository;
using Shared.Interfaces;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace News.Api.ODataControllers
{
    [ODataRoutePrefix("newsattachments")]
    public class NewsAttachmentController : ODataController
    {
        #region Fields
        private readonly ILogger<NewsAttachmentController> _logger;
        private readonly IEntityService<Models.NewsAttachment, int, NewsContext> _entityService;
        #endregion

        #region Constructor
        public NewsAttachmentController(
            ILogger<NewsAttachmentController> logger,
            IEntityService<Models.NewsAttachment, int, NewsContext> entityService
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
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IQueryable<Models.NewsAttachment>))]
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
        [ODataRoute("")]
        [SwaggerOperation("Search")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IQueryable<Models.NewsAttachment>))]
        public async Task<IActionResult> Get()
        {
            return Ok(await _entityService.GetQuery());
        }

        [EnableQuery]
        [ODataRoute("({key})")]
        [SwaggerOperation(nameof(Patch))]
        [AcceptVerbs("PATCH", "MERGE")]
        [ProducesResponseType((int)HttpStatusCode.Accepted, Type = typeof(Models.NewsAttachment))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Patch([FromODataUri] int key, Delta<Models.NewsAttachment> model)
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
        [ProducesResponseType((int)HttpStatusCode.Accepted, Type = typeof(Models.NewsAttachment))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Put([FromODataUri] int key, Models.NewsAttachment model)
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
        [ProducesResponseType((int)HttpStatusCode.Created, Type = typeof(Models.NewsAttachment))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Post(Models.NewsAttachment model)
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
                var msg = $"Unable to delete NewsAttachment with id {key}";
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
