using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Threading.Tasks;
using Timereg.Api.Domain.Constants;
using Timereg.Api.Domain.Interfaces;
using Timereg.Api.Domain.Models;
using contracts = Timereg.Api.Contracts;

namespace Timereg.Api.Controllers
{
    [ApiController]
    [Route("absenceexports")]
    public class AbsenceExportController : Controller
    {
        private readonly IAbsenceExportService _absenceExportService;
        private readonly IMapper _mapper;
        private readonly IAbsenceService _absenceService;

        public AbsenceExportController(IAbsenceExportService absenceExportService,
            IMapper mapper,
            IAbsenceService absenceService)
        {
            _absenceExportService = absenceExportService;
            _mapper = mapper;
            _absenceService = absenceService;
        }

        [HttpGet]
        [SwaggerOperation("GetAll")]
        public async Task<ActionResult<IEnumerable<contracts.AbsenceExport>>> GetAbsenceExports([FromQuery]contracts.SearchQueryAbsenceExport query)
        {
            var mapQuery = _mapper.Map<contracts.SearchQueryAbsenceExport, SearchQueryAbsenceExport>(query);

            var absenceExports = await _absenceExportService.Search(mapQuery);
            var absenceExportContracts = _mapper.Map<IEnumerable<contracts.AbsenceExport>>(absenceExports);

            return Ok(absenceExportContracts);
        }

        [HttpGet("{absenceExportId}")]
        [SwaggerOperation("Execute")]
        public async Task<ActionResult> Execute(string absenceExportId, string action)
        {
            if (action == AbsenceExportActions.Resend)
            {
                await _absenceService.Resend(absenceExportId);
            }
            return Ok();
        }
    }
}

