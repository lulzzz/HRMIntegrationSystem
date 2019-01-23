using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Altinn.Api.Domain.Entities;
using Altinn.Api.Domain.Interfaces;

namespace Altinn.Api.Repositories.Repositories
{
    public class ExternalSystemRepository : IRepository<ExternalSystem>
    {
        public async Task<IEnumerable<ExternalSystem>> Search()
        {
            return new List<ExternalSystem>()
            {
                new ExternalSystem { Id = new Guid("2FF42A64-2A36-42F1-9923-70898F7A4F1F"), SpecificValue = ExternalGovernmentSystem.DigiSyfo, Order = 2, Image = "digisyfo-logo.png" },
            };
        }
    }
}