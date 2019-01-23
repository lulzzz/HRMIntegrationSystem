using System;
using Integrations.Api.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Integrations.Api.Domain.Interfaces;

namespace Integration.Api.Repositories.Repositories
{
    public class IntegrationCategoryRepository : IIntegrationCategoryRepository
    {
        public async Task<List<IntegrationCategory>> GetCategories()
        {
            return new List<IntegrationCategory>()
            {
                new IntegrationCategory{ Id = new Guid("A20543FE-D882-4AED-9452-07D9EF262D75"), SpecificValue = IntegrationCategoryEnum.Timereg, Order = 1},
             // new IntegrationCategory{ Id = new Guid("94E3B90A-1744-4A49-BE28-EAAA9C295DDE"), SpecificValue = IntegrationCategoryEnum.Goverment, Order = 2},
            };
        }
    }
}
