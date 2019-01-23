using System.Collections.Generic;
using System.Threading.Tasks;

using Altinn.Api.Domain.Entities;
using Shared.Contracts;

namespace Altinn.Api.Domain.Interfaces
{
    public interface IExternalSystemService
    {
        Task<IEnumerable<ExternalSystem>> Search(SearchQueryExternalSystem query);
        Task<IEnumerable<ExternalData>> GetExternalData(ExternalGovernmentSystem id);
    }
}
