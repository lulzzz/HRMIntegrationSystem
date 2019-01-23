using System.Threading.Tasks;
using Common.Api.Domain.Entities;
using Common.Api.Domain.Interfaces;
using Shared.Exceptions;

namespace Common.Api.Domain.Services
{
    public class BrregService : IBrregService
    {
        private readonly IBrregRepository _brregRepository;
        
        public BrregService(IBrregRepository brregRepository)
        {
            _brregRepository = brregRepository;
        }

        public async Task<BrregEntity> GetBrregEntity(int organizationNumber, bool includeChildren)
        {
            var parent = await _brregRepository.LookupBrregEntity(organizationNumber);

            if (parent == null)
            {
                var child = await _brregRepository.LookupBrregChildEntity(organizationNumber);

                if (child == null)
                    throw new NotFoundException("Organisasjonen ble ikke funnet");
                return child;
            }

            if (!includeChildren) return parent;

            var children = await _brregRepository.LookupBrregChildren(organizationNumber);

            parent.Children = children;

            return parent;
        }
    }
}