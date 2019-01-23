using System.Threading.Tasks;
using Common.Api.Domain.Entities;

namespace Common.Api.Domain.Interfaces
{
    public interface IBrregService
    {
        Task<BrregEntity> GetBrregEntity(int organizationNumber, bool includeChildren);
    }
}