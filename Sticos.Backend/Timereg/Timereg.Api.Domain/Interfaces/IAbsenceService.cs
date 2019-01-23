using System.Collections.Generic;
using System.Threading.Tasks;
using Timereg.Api.Domain.Models;

namespace Timereg.Api.Domain.Interfaces
{
    public interface IAbsenceService
    {
        Task ExportAbsence(Absence absence);
        Task DeleteAbsence(Absence absence);
        Task Resend(string absenceExportId);
    }
}
