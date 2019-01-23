using Shared.Domain.Enums;
using Shared.Interfaces;
using System;
using System.Threading.Tasks;

namespace News.Api.Services
{
    public class NewsAuthorizationService : IEntityAuthorizationService<Models.News, int>
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly ICurrentUserContext _currentUserContext;

        public NewsAuthorizationService(
            IAuthorizationService authorizationService,
            ICurrentUserContext currentUserContext
            )
        {
            _authorizationService = authorizationService ?? throw new ArgumentNullException(nameof(authorizationService));
            _currentUserContext = currentUserContext ?? throw new ArgumentNullException(nameof(currentUserContext));
        }

        public async Task<bool> CanCreate(Models.News entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var currentUserId = _currentUserContext.Get().UserId;
            if (currentUserId <= 0) throw new ArgumentOutOfRangeException(nameof(currentUserId));

            return await _authorizationService.HasAnyPermission(currentUserId, entity.UnitId.GetValueOrDefault(UnitConstants.MasterUnitId), PermissionType.SkrivNyheter);
        }

        public async Task<bool> CanUpdate(Models.News entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var currentUserId = _currentUserContext.Get().UserId;
            if (currentUserId <= 0) throw new ArgumentOutOfRangeException(nameof(currentUserId));

            return await _authorizationService.HasAnyPermission(currentUserId, entity.UnitId.GetValueOrDefault(UnitConstants.MasterUnitId), PermissionType.SkrivNyheter);
        }

        public async Task<bool> CanDelete(Models.News entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var currentUserId = _currentUserContext.Get().UserId;
            if (currentUserId <= 0) throw new ArgumentOutOfRangeException(nameof(currentUserId));

            return await _authorizationService.HasAnyPermission(currentUserId, entity.UnitId.GetValueOrDefault(UnitConstants.MasterUnitId), PermissionType.SkrivNyheter);
        }
    }
}
