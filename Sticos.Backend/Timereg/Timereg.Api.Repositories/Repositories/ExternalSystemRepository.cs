using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timereg.Api.Domain.Interfaces;
using Timereg.Api.Domain.Models;

namespace Timereg.Api.Repositories.Repositories
{
    public class ExternalSystemRepository : IExternalSystemRepository
    {
        private readonly List<ExternalSystem> _externalSystems;
        public ExternalSystemRepository()
        {
            _externalSystems = new List<ExternalSystem>()
        {
          new ExternalSystem { Id = new Guid("3CF42A64-2A36-42F1-9923-70898F7A4F1F"), SpecificValue = ExternalEconomySystem.UniMicro, Order = 2, Image = "unimicro-logo.png", WebsiteUrl = "https://unieconomy.no/"  }
        };
        }

        public async Task<ExternalSystem> GetExternalSystem(int externalSystem)
        {
            return _externalSystems.FirstOrDefault(x => (int)x.SpecificValue == externalSystem);
        }

        public async Task<List<ExternalSystem>> Search()
        {
            return _externalSystems;
        }
    }
}
