using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

using Altinn.Api.Domain.Entities;
using Altinn.Api.Domain.Interfaces;
using Altinn.Api.Repositories.Context;

using Shared.Interfaces;
using dbModels = Altinn.Api.Repositories.Models;

namespace Altinn.Api.Repositories
{
    public class NavMessageRepository : IRepository<NavMessage, SearchQueryNavMessage>
    {
        private readonly IDbContextFactory<AltinnDbContext> _contextFactory;
        private readonly IMapper _mapper;

        public NavMessageRepository(IDbContextFactory<AltinnDbContext> contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<NavMessage> Get(int id)
        {
            using (var context = await _contextFactory.CreateDbContext())
            {
                var entity = await context.NavMessages
                    .SingleOrDefaultAsync(d => d.Id == id);

                var navMessage = _mapper.Map<dbModels.NavMessage, NavMessage>(entity);

                return navMessage;
            }
        }

        public async Task<NavMessage> Save(NavMessage message)
        {
            var entitytoCreate = _mapper.Map<NavMessage, dbModels.NavMessage>(message);

            using (var context = await _contextFactory.CreateDbContext())
            {
                context.NavMessages.Add(entitytoCreate);
                await context.SaveChangesAsync();
            }

            var createNavMessage = _mapper.Map<dbModels.NavMessage, NavMessage>(entitytoCreate);
            return createNavMessage;
        }

        public async Task<IEnumerable<NavMessage>> Search(SearchQueryNavMessage searchParameters)
        {
            using (var context = await _contextFactory.CreateDbContext())
            {
                var query = context.NavMessages.AsQueryable();

                if (!string.IsNullOrWhiteSpace(searchParameters.Namespace))
                {
                    query = query.Where(x => x.Namespace.ToLower().Contains(searchParameters.Namespace.ToLower()));
                }

                if (searchParameters.IntegrationType.HasValue)
                {
                    query = query.Where(msg => msg.IntegrationType == searchParameters.IntegrationType);
                }

                if (searchParameters.WorkState.HasValue)
                {
                    query = query.Where(msg => msg.WorkState == searchParameters.WorkState);
                }

                if (!string.IsNullOrWhiteSpace(searchParameters.BusinessOrganizationNumber))
                {
                    query = query.Where(x => x.BusinessOrganizationNumber.Contains(searchParameters.BusinessOrganizationNumber));
                }

                query = query
                    .Skip(searchParameters.Skip)
                    .Take(searchParameters.Take);

                var filteredNavMessages = await query.ToListAsync();
                var mapNavMessages = _mapper.Map<IEnumerable<NavMessage>>(filteredNavMessages);

                return mapNavMessages.ToList();
            }
        }
    }
}