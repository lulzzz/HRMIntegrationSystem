using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Api.Domain.Entities;
using Common.Api.Domain.Interfaces;
using Common.Api.Domain.Interfaces.Repositories;

namespace Common.Api.Domain.Services
{
    public class OwnerTypeService : IOwnerTypeService
    {
        private readonly IRepository<OwnerType, SearchQueryOwnerType> _ownerTypeRepository;

        public OwnerTypeService(IRepository<OwnerType, SearchQueryOwnerType> ownerTypeRepository)
        {
            _ownerTypeRepository = ownerTypeRepository;
        }

        public async Task<IEnumerable<OwnerType>> Search(SearchQueryOwnerType query)
        {
            return await _ownerTypeRepository.Search(query);
        }
    }
}