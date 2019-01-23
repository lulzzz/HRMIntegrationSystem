using AutoMapper;
using Integrations.Api.Repositories.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.Interfaces;
using db = Integrations.Api.Repositories.Models;
using domain = Integrations.Api.Domain.Models;


namespace Integrations.Api.Repositories.Repositories
{
    public class IntegrationRepository : Integrations.Api.Domain.Interfaces.IRepository<domain.Integration, domain.SearchQueryIntegration>
    {
        private readonly IDbContextFactory<IntegrationDbContext> _contextFactory;
        private readonly IMapper _mapper;

        public IntegrationRepository(IDbContextFactory<IntegrationDbContext> contextFactory,
                                     IMapper mapper)
        {
            _mapper = mapper;
            _contextFactory = contextFactory;
        }

        public async Task<Domain.Models.Integration> Create(domain.Integration entity)
        {
            var mappedEntity = _mapper.Map<domain.Integration, db.Integration>(entity);
            using (var context = await _contextFactory.CreateDbContext())
            {
                context.Integrations.Add(mappedEntity);
                await context.SaveChangesAsync();

                var addedEntity = _mapper.Map<db.Integration, domain.Integration>(mappedEntity);
                return addedEntity;
            }
        }

        public async Task<IList<domain.Integration>> CreateAll(IEnumerable<domain.Integration> entities)
        {
            var mappedEntities = _mapper.Map<IEnumerable<domain.Integration>, IEnumerable<db.Integration>>(entities);
            using (var context = await _contextFactory.CreateDbContext())
            {
                context.Integrations.AddRange(mappedEntities);
                await context.SaveChangesAsync();
                var addedEntities = _mapper.Map<IEnumerable<db.Integration>, IEnumerable<domain.Integration>>(mappedEntities);

                return addedEntities.ToList();
            }
        }

        public async Task<domain.Integration> GetSingle(int id)
        {
            using (var context = await _contextFactory.CreateDbContext())
            {
                var entity = await context.Integrations.FirstOrDefaultAsync(x => x.Id == id);
                var mappedEntity = _mapper.Map<db.Integration, domain.Integration>(entity);
                return mappedEntity;
            }
        }

        public async Task<domain.Integration> Update(domain.Integration entity)
        {
            var mappedEntity = _mapper.Map<domain.Integration, db.Integration>(entity);
            using (var context = await _contextFactory.CreateDbContext())
            {
                context.Integrations.Update(mappedEntity);
                await context.SaveChangesAsync();
                return entity;
            }
        }

        public async Task<IList<domain.Integration>> Search(domain.SearchQueryIntegration searchQuery)
        {
            using (var context = await _contextFactory.CreateDbContext())
            {
                var query = context.Integrations.AsQueryable();

                if(searchQuery.UnitId != 0 || searchQuery.UnitIds.Any())
                {
                    query = query.Where(x => (searchQuery.UnitId != 0 && x.UnitId == searchQuery.UnitId) || searchQuery.UnitIds.Contains(x.UnitId));
                }

                if (searchQuery.ExternalSystemId != 0)
                    query = query.Where(x => x.ExternalSystem == searchQuery.ExternalSystemId);

                if (searchQuery.Category != 0)
                    query = query.Where(x => x.Category == searchQuery.Category);

                var result = _mapper.Map<IEnumerable<domain.Integration>>(query);
                return result.ToList();
            }
        }

        public async Task<IList<domain.Integration>> UpdateAll(IEnumerable<domain.Integration> entities)
        {
            var mappedEntities = _mapper.Map<IEnumerable<domain.Integration>, IEnumerable<db.Integration>>(entities);
            using (var context = await _contextFactory.CreateDbContext())
            {
                context.Integrations.UpdateRange(mappedEntities);
                await context.SaveChangesAsync();
                return entities.ToList();
            }
        }

        public Task<IList<domain.Integration>> CreateAll(IList<domain.Integration> entities)
        {
            throw new System.NotImplementedException();
        }

        public Task<IList<domain.Integration>> UpdateAll(IList<domain.Integration> entities)
        {
            throw new System.NotImplementedException();
        }

        public async Task<domain.Integration> Delete(int id)
        {
            using(var context = await _contextFactory.CreateDbContext())
            {
                var integrationForDelete =  await context.Integrations.FirstOrDefaultAsync(x => x.Id == id);

                if (integrationForDelete != null)
                {
                    context.Integrations.Remove(integrationForDelete);
                    var entityMapsForDelete = await context.EntityMaps.Where(x => x.IntegrationId == id).ToListAsync();
                    context.EntityMaps.RemoveRange(entityMapsForDelete);
                    await context.SaveChangesAsync();
                }

                return _mapper.Map<domain.Integration>(integrationForDelete);
            }
        }
    }
}
