using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.Interfaces;
using Integrations.Api.Domain.Interfaces;
using Integrations.Api.Domain.Models;
using Integrations.Api.Repositories.Context;
using entities = Integrations.Api.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace Integrations.Api.Repositories.Repositories
{
    public class EntityMapRepository : IEntityMapRepository
    {
        private readonly IMapper _mapper;
        private readonly IDbContextFactory<IntegrationDbContext> _contextFactory;

        public EntityMapRepository(IMapper mapper, IDbContextFactory<IntegrationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<IList<EntityMap>> Search(SearchQueryEntityMap searchQuery)
        {
            if (searchQuery.IntegrationId <= 0) return new List<EntityMap>();

            using (var context = await _contextFactory.CreateDbContext())
            {
                var query = context.EntityMaps.AsQueryable();
                query = AddWhereClauses(searchQuery, query);

                var result = _mapper.Map<IList<EntityMap>>(query);
                return result;
            }
        }


        public async Task RemoveAllMatching(IEnumerable<EntityMap> entities)
        {
            using (var context = await _contextFactory.CreateDbContext())
            {
                var recordsToBeDeleted = context.EntityMaps.Where(x => entities.Any(e => e.EntityId == x.EntityId
                                                                                         && e.IntegrationId == x.IntegrationId
                                                                                         && e.EntityName == x.EntityName));
                // Delete current mappings
                context.EntityMaps.RemoveRange(recordsToBeDeleted);
                await context.SaveChangesAsync();
            }
        }
        public async Task<IList<EntityMap>> Add(IEnumerable<EntityMap> entities)
        {
            entities = entities.Where(x => !x.Deleted);
            var mappedEntities = _mapper.Map<IEnumerable<EntityMap>, IEnumerable<entities.EntityMap>>(entities);
            using (var context = await _contextFactory.CreateDbContext())
            {
                await context.EntityMaps.AddRangeAsync(mappedEntities);
                await context.SaveChangesAsync();
                var updatedMaps = _mapper.Map<IEnumerable<entities.EntityMap>, IEnumerable<EntityMap>>(mappedEntities);
                return updatedMaps.ToList();
            }
        }

        private static IQueryable<entities.EntityMap> AddWhereClauses(SearchQueryEntityMap searchQuery, IQueryable<entities.EntityMap> query)
        {
            query = query.Where(x => x.IntegrationId == searchQuery.IntegrationId);

            if (searchQuery.LocalId > 0)
            {
                query = query.Where(x => x.EntityId == searchQuery.LocalId);
            }

            if (!string.IsNullOrWhiteSpace(searchQuery.EntityName))
            {
                query = query.Where(x => x.EntityName == searchQuery.EntityName);
            }

            if (!string.IsNullOrEmpty(searchQuery.ExternalEntity))
            {
                query = query.Where(x => x.ExternalEntity == searchQuery.ExternalEntity);
            }

            if (!string.IsNullOrEmpty(searchQuery.ExternalProperty))
            {
                query = query.Where(x => x.ExternalPropertyName == searchQuery.ExternalProperty);
            }

            return query;
        }

        public async Task DeleteEmployee(EmployeeDeleted message)
        {
            using (var context = await _contextFactory.CreateDbContext())
            {
                context.EntityMaps.RemoveRange(context.EntityMaps.Where(x => x.EntityId == message.EmployeeId 
                                                                        && x.EntityName == EntityType.Employee.ToString()));
                await context.SaveChangesAsync();
            }
        }
    }
}
