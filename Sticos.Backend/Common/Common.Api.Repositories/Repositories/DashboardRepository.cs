using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Common.Api.Domain.Entities;
using Common.Api.Domain.Interfaces.Repositories;
using Common.Api.Repositories.Context;
using Common.Api.Repositories.ContextFactory;
using Microsoft.EntityFrameworkCore;
using Shared.Interfaces;


namespace Common.Api.Repositories.Repositories
{
    public class DashboardRepository : IRepository<Dashboard, SearchQueryDashboard>
    {
        private readonly IDbContextFactory<SticosWidgetDbContext> _contextFactory;
        private readonly IMapper _mapper;

        public DashboardRepository(
            IDbContextFactory<SticosWidgetDbContext> contextFactory,
            IMapper mapper
        )
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<Dashboard> Create(Dashboard dashboard)
        {
            var entityToCreate = _mapper.Map<Dashboard, Models.Dashboard>(dashboard);
            entityToCreate.DateCreated = DateTimeOffset.Now;

            using (var context = await _contextFactory.CreateDbContext())
            {
                context.Dashboards.Add(entityToCreate);
                await context.SaveChangesAsync();
            }

            var createDashboard = _mapper.Map<Models.Dashboard, Dashboard>(entityToCreate);
            return createDashboard;
        }

        public async Task<Dashboard> Update(Dashboard dashboard)
        {
            var entityToUpdate = _mapper.Map<Dashboard, Common.Api.Repositories.Models.Dashboard>(dashboard);

            using (var context = await _contextFactory.CreateDbContext())
            {
                var entity = context.Dashboards.AsNoTracking().SingleOrDefault(x => x.Id == entityToUpdate.Id);
                entityToUpdate.DateCreated = entity.DateCreated;
                entityToUpdate.IsDefault = entity.IsDefault;
                context.Dashboards.Update(entityToUpdate);
                await context.SaveChangesAsync();
            }

            var updateDashboard = _mapper.Map<Common.Api.Repositories.Models.Dashboard, Dashboard>(entityToUpdate);
            return updateDashboard;
        }

        public async Task<Dashboard> Delete(int id)
        {
            using (var context = await _contextFactory.CreateDbContext())
            {
                var entityToDelete = context.Dashboards.AsNoTracking().SingleOrDefault(x => x.Id == id);
                if (entityToDelete != null)
                {
                    context.Dashboards.Remove(entityToDelete);
                    await context.SaveChangesAsync();
                }

                var deleteDashboard =
                        _mapper.Map<Common.Api.Repositories.Models.Dashboard, Dashboard>(entityToDelete);
                return deleteDashboard;
            }
        }

        public async Task<Dashboard> GetById(int id)
        {
            using (var context = await _contextFactory.CreateDbContext())
            {
                var entity = await context.Dashboards
                    .Include(d => d.OwnerType)
                    .SingleOrDefaultAsync(d => d.Id == id);

                var dashboard = _mapper.Map<Common.Api.Repositories.Models.Dashboard, Dashboard>(entity);
                return dashboard;
            }
        }

        public async Task<IList<Dashboard>> Search(SearchQueryDashboard searchQuery)
        {
            using (var context = await _contextFactory.CreateDbContext())
            {
                var query = context.Dashboards
                    .Where(x => x.OwnerId == searchQuery.UserId)
                    .AsQueryable();

                var userDashboards = await query.ToListAsync();

                if (userDashboards.Count == 0)
                {
                    var ownerTypes = context.OwnerTypes.ToList();
                    var nonUserOwnerTypes = ownerTypes.Where(x => x.Name != "User").ToList();
                    var userOwnerType = ownerTypes.First(x => x.Name == "User");
                    var defaultDashboards = context.Dashboards
                        .Include(x => x.OwnerType)
                        .Where(x => x.OwnerType.Name != "User")
                        .OrderBy(x => x.OwnerType.Priority)
                        .ToList();

                    Models.Dashboard defaultConfig = defaultDashboards.FirstOrDefault();

                    var userDefaultConfig = new Dashboard
                    {
                        Title = "Default",
                        DashboardConfig = defaultConfig?.DashboardConfig,
                        OwnerId = searchQuery.UserId,
                        OwnerTypeId = userOwnerType.Id,
                        IsDefault = true
                    };

                    await Create(userDefaultConfig);
                }

                query = query
                        .Skip(searchQuery.Skip ?? SearchConstants.DEFAULT_SKIP)
                        .Take(searchQuery.Take ?? SearchConstants.DEFAULT_TAKE)
                    .OrderBy(x => x.DateCreated);

                var filterResult = await query.ToListAsync();
                var result = _mapper.Map<IEnumerable<Dashboard>>(filterResult);
                return result.ToList();
            }
        }

        public async Task<bool> Exists(int id)
        {
            using (var context = await _contextFactory.CreateDbContext())
            {
                return context.Dashboards.Any(x => x.Id == id);
            }
        }
    }
}