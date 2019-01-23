using System.Collections.Generic;
using System.Threading.Tasks;
using Timereg.Api.Domain.Models;

namespace Timereg.Api.Domain.Interfaces
{
    public interface IAbsenceExportService
    {
        Task<IEnumerable<AbsenceExport>> Search(SearchQueryAbsenceExport searchQuery);
        Task<AbsenceExport> CreateAbsenceExport(AbsenceExport absenceExport);
        Task<AbsenceExport> UpdateAbsenceExport(AbsenceExport absenceExport);
        Task Delete(int unitId);
        Task DeleteEmployee(EmployeeDeleted message);
    }
}
