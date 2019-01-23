using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using Integrations.Api.Domain.Interfaces;
using Shared.Contracts;

namespace Integrations.Api.Controllers
{
    [ApiController]
    [Route("integrations/categories")]
    public class IntegrationCategoryController : Controller
    {
        IIntegrationCategoryService _categoryService;

        public IntegrationCategoryController(IIntegrationCategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [SwaggerOperation("Get")]
        public async Task<ActionResult<IEnumerable<ICode>>> GetCategories()
        {
            var categories = await _categoryService.GetCategories();
            return Ok(categories);
        }
    }
}