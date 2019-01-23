using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Altinn.Api.Domain.Entities;
using Altinn.Api.Domain.Interfaces;
using Shared.Contracts;

namespace Altinn.Api.Services
{
    public class ExternalSystemService : IExternalSystemService
    {
        private readonly IRepository<ExternalSystem> _repository;
        private readonly IExternalSystemFactory _externalSystemFactory;

        public ExternalSystemService(IRepository<ExternalSystem> repository, IExternalSystemFactory externalSystemFactory)
        {
            _repository = repository;
            _externalSystemFactory = externalSystemFactory;
        }

        public async Task<IEnumerable<ExternalData>> GetExternalData(ExternalGovernmentSystem id)
        {
            var externalAdapter = _externalSystemFactory.CreateDataService(id);
            return await externalAdapter.GetExternalReportees();
        }

        public async Task<IEnumerable<ExternalSystem>> Search(SearchQueryExternalSystem query)
        {
            var externalSystems = await _repository.Search();
            return externalSystems.OrderBy(e => e.Order);
        }
    }
}
