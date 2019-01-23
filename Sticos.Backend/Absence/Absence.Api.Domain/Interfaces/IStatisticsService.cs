
using Shared.Domain.Interfaces;
using System.Threading.Tasks;

namespace Absence.Api.Domain.Interfaces
{
    public interface IStatisticsService
    {
        Task<IChart> GetStatistics(int id);
    }
}
