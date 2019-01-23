using Integrations.Api.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Integrations.Api.Domain.Interfaces
{
    public interface IIntegrationCategoryService
    {
        Task<IEnumerable<IntegrationCategory>> GetCategories();
    }
}
