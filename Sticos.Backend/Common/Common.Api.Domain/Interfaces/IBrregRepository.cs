using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Api.Domain.Entities;

namespace Common.Api.Domain.Interfaces
{
    public interface IBrregRepository
    {
        Task<BrregEntity> LookupBrregEntity(int orgNumber);
        Task<BrregEntity> LookupBrregChildEntity(int orgNumber);
        Task<List<BrregEntity>> LookupBrregChildren(int organizationNumber);
    }
}