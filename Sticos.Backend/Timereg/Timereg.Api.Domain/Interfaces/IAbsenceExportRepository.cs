using Sticos.Personal.MessageContracts;
using System.Threading.Tasks;
using Timereg.Api.Domain.Models;

namespace Timereg.Api.Domain.Interfaces
{
    public interface IAbsenceExportRepository : IRepository<AbsenceExport, SearchQueryAbsenceExport>
    {
        Task Delete(int unitId);
        Task DeleteEmployee(EmployeeDeleted employeeId);
    }
}
