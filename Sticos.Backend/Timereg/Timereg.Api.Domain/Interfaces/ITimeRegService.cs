using System.Threading.Tasks;
using Timereg.Api.Domain.Models;

namespace Timereg.Api.Domain.Interfaces
{
    public interface ITimeRegService
    {
        Task<HourBalance> GetHourBalance(int unitId, int employeeId);
    }
}
