using System.Collections.Generic;
using System.Threading.Tasks;
using Timereg.Api.Domain.Models;

namespace Timereg.Api.Domain.Interfaces
{
    public interface IExternalSystemAdapter
    {
        Task<HourBalance> GetHourBalance(int unitId, int employeeId);
        Task<Absence> ExportAbsence(int unitId,Absence absence);
        Task DeleteAbsence(int unitId, Absence absence);
        Task<Absence> ResendAbsence(int unitId, Absence absence);
    }
}
