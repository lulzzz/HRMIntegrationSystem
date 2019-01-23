using System.Linq;
using System.Threading.Tasks;
using Integrations.Api.Contracts;
using Integrations.Api.Contracts.Services;
using Shared.Interfaces;
using Timereg.Api.Domain.AdaptersValidators.Interfaces;
using Timereg.Api.Domain.Models;
using Timereg.Api.Domain.Validators.Interfaces;
using Timereg.Api.Domain.Validators.Models;
using Timereg.Api.Unimicro.Adapters.UniMicro;
using integrationsContracts = Integrations.Api.Contracts;

namespace Timereg.Api.Unimicro.Validators
{
    public class UnimicroValidator : IValidateAdapter
    {
        private readonly IIntegrationService _integrationService;
        private readonly IEntityMapService _entityMapService;

        public UnimicroValidator(IIntegrationService integrationService, IEntityMapService entityMapService)
        {
            _integrationService = integrationService;
            _entityMapService = entityMapService;
        }

        public async Task<ITimeregValidationResult> ValidateExportAbsence(int unitId, Absence absence)
        {   
            foreach (var absenceEntry in absence.AbsenceEntries)
            {
                int? workTypeId = null;
                int? employmentLeaveTypeId = null;

                workTypeId = await _entityMapService.GetWorkTypeId(absenceEntry.LocalAbsenceCode, unitId);
                if (workTypeId.HasValue)
                {
                    var workRelationId = await _entityMapService.GetWorkRelationId(absence.EmployeeId, unitId);
                    if (!workRelationId.HasValue)
                    {
                        return new FailedResult($"Missing mapping from Employee to WorkRelation {absence.EmployeeId}");
                    }
                }
                else
                {
                    employmentLeaveTypeId = await _entityMapService.GetEmploymentLeaveId(absenceEntry.LocalAbsenceCode, unitId);
                    if (employmentLeaveTypeId.HasValue)
                    {
                        var employmentId = await _entityMapService.GetEmploymentId(absence.EmployeeId, unitId);
                        if (!employmentId.HasValue)
                        {
                            return new FailedResult($"Missing mapping from Employee to Employment {absence.EmployeeId}");
                        }
                    }
                }

                if (!workTypeId.HasValue && !employmentLeaveTypeId.HasValue)
                {
                    return new FailedResult($"Missing mapping from AbsenceType to WorkType or EmploymentLeaveType {absenceEntry.LocalAbsenceCode}");
                }
            }
            return new OkResult("Absence validated OK for export");
        }


        public async Task<ITimeregValidationResult> ValidateHourBalance(int unitId, int employeeId)
        {
            var workRelationId = await _entityMapService.GetWorkRelationId(employeeId, unitId);
            
            if (!workRelationId.HasValue)
            {
                return new FailedResult($"Missing mapping from Employee to WorkRelation. {employeeId}");
            }

            return new OkResult("Validation for HourBalance is OK");
        }
    }
}
