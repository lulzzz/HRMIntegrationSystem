using Integrations.Api.Contracts;
using System.Threading.Tasks;
using Timereg.Api.Domain.Validators.Interfaces;
using domain = Timereg.Api.Domain.Models;

namespace Timereg.Api.Domain.AdaptersValidators.Interfaces
{
    public interface IValidateAdapter
    {
        Task<ITimeregValidationResult> ValidateExportAbsence(int unitId,domain.Absence entity);
        Task<ITimeregValidationResult> ValidateHourBalance(int unitId, int employeeId);
    }
}
