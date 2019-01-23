using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Api.Domain.Entities;
using Common.Api.Domain.Interfaces;
using Common.Api.Domain.Interfaces.Repositories;

namespace Common.Api.Domain.Services
{
    public class UnitService : IUnitService
    {
        private readonly IUnitRepository _repository;

        public UnitService(IUnitRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Unit>> Search(SearchQueryUnit query)
        {
            return await _repository.Search(query);
        }

        public async Task<Unit> GetUnit(int id)
        {
            var unit = await _repository.GetUnit(id);
            return unit;
        }
    }
}
