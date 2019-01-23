using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Domain;
using Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Services.Services
{
    public class EntityService<T, TId, TContext> : IEntityService<T, TId, TContext>
        where T : EntityBase<T, TId>
        where TContext : DbContext
    {
        private const int MAX_IDFILTER_COUNT = 5000;

        private readonly ILogger<EntityService<T, TId, TContext>> _logService;
        private readonly IDbContextFactory<TContext> _dbContextFactory;
        private readonly IEntityFilterService<T, TId> _filterService;
        private readonly ICurrentUserContext _currentUserContext;
        private readonly IAuthorizationService _authorizationService;
        private readonly IEntityAuthorizationService<T, TId> _entityAuthorizationService;

        public EntityService(
            ILogger<EntityService<T, TId, TContext>> logService,
            IDbContextFactory<TContext> dbContextFactory,
            IEntityFilterService<T, TId> filterService,
            ICurrentUserContext currentUserContext,
            IAuthorizationService authorizationService,
            IEntityAuthorizationService<T, TId> entityAuthorizationService)
        {
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));
            _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
            _filterService = filterService ?? throw new ArgumentNullException(nameof(filterService));
            _currentUserContext = currentUserContext ?? throw new ArgumentNullException(nameof(currentUserContext));
            _authorizationService = authorizationService ?? throw new ArgumentNullException(nameof(authorizationService));
            _entityAuthorizationService = entityAuthorizationService ?? throw new ArgumentNullException(nameof(entityAuthorizationService));
        }

        public async Task<IQueryable<T>> GetQuery()
        {
            var query = await GetBaseQuery();

            return query;
        }

        public async Task<T> GetById(TId id)
        {
            if (EqualityComparer<TId>.Default.Equals(id, default(TId))) throw new ArgumentNullException(nameof(id));

            var query = await GetBaseQuery();
            var entity = await query.Where(x => Equals(x.Id, id)).FirstOrDefaultAsync();

            return entity;
        }

        public async Task<T> Create(T entity)
        {
            if (EqualityComparer<T>.Default.Equals(entity, default(T))) throw new ArgumentNullException(nameof(entity));

            var canCreate = await _entityAuthorizationService.CanCreate(entity);
            if (!canCreate) throw new UnauthorizedAccessException();

            var dbContext = await _dbContextFactory.CreateDbContext();
            var createdEntity = await dbContext.AddAsync(entity);

            var saveTracking = await dbContext.SaveChangesAsync();

            return createdEntity.Entity;
        }

        public async Task<T> Update(T entity)
        {
            if (EqualityComparer<T>.Default.Equals(entity, default(T))) throw new ArgumentNullException(nameof(entity));

            var canUpdate = await _entityAuthorizationService.CanUpdate(entity);
            if (!canUpdate) throw new UnauthorizedAccessException();

            var dbContext = await _dbContextFactory.CreateDbContext();
            var updatedEntity = dbContext.Update(entity);

            var saveTracking = await dbContext.SaveChangesAsync();

            return updatedEntity.Entity;
        }

        public async Task<bool> Delete(TId id)
        {
            if (EqualityComparer<TId>.Default.Equals(id, default(TId))) throw new ArgumentNullException(nameof(id));

            var entity = await GetById(id);
            if (entity == null)
            {
                _logService.LogWarning($"Could not find entity with id {id}");
                return false;
            }

            var canDelete = await _entityAuthorizationService.CanDelete(entity);
            if (!canDelete) throw new UnauthorizedAccessException();

            var dbContext = await _dbContextFactory.CreateDbContext();
            var deletedEntity = dbContext.Remove(entity);

            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logService.LogError(ex, $"Unable to delete entity id {id}");
                return false;
            }

            return true;
        }

        private async Task<IQueryable<T>> GetBaseQuery()
        {
            var currentUser = _currentUserContext.Get();
            if (currentUser == null) throw new ArgumentNullException(nameof(currentUser));

            int currentUserId = currentUser.UserId;
            if (currentUserId <= 0) throw new ArgumentOutOfRangeException($"{nameof(currentUserId)}: {currentUserId}");

            var dbContext = await _dbContextFactory.CreateDbContext();

            var query = dbContext.Set<T>().AsQueryable();

            var isCustomerAdmin = await _authorizationService.IsCustomerAdmin(currentUserId);
            if (isCustomerAdmin)
            {
                return query;
            }

            var baseQuery = await _filterService.GetBaseFilter();
            if (baseQuery != null)
            {
                query = baseQuery.AsQueryable();
            }

            var idList = await _filterService.GetIdFilter();
            if (!idList.Any())
            {
                return query;
            }

            var count = idList.Count();
            if (count > 2000)
            {
                _logService.LogWarning($"Entity idfilter: {nameof(T)}: {count}");
            }

            if (count > MAX_IDFILTER_COUNT)
            {
                //Check if we can add a negating query here. NOT IN() might have less items

                //TODO: Add bulk insert workaround here
                _logService.LogCritical($"Entity idfilter: {nameof(T)}: {count}");
                return query;
            }

            return query.Where(x => idList.Contains(x.Id));
        }
    }
}
