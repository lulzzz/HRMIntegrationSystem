using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Api.Domain.Entities;

namespace Common.Api.Domain.Interfaces
{
    public interface IOwnerTypeService
    {
        Task<IEnumerable<OwnerType>> Search(SearchQueryOwnerType query);
    }
}