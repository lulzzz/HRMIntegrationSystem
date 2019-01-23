using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Altinn.Api.Domain.Interfaces;
using Shared.Contracts;

namespace Altinn.Api.Services
{
    public class AltinnMatchingService : IExternalMatchingService
    {
        public AltinnMatchingService()
        {

        }

        public async Task<IEnumerable<EntityMatch>> MatchUnitData(int unitId, int[] ids)
        {
            throw new NotImplementedException();
        }
    }
}
