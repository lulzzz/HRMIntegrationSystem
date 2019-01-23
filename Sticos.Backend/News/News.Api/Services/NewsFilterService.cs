using Microsoft.Extensions.Logging;
using News.Api.Repository;
using Shared.Domain.Enums;
using Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace News.Api.Services
{
    public class NewsFilterService : IEntityFilterService<Models.News, int>
    {
        private readonly ILogger<NewsFilterService> _logService;
        private readonly IDbContextFactory<NewsContext> _dbContextFactory;
        private readonly ICurrentUserContext _currentUserContext;
        private readonly IAuthorizationService _authorizationService;

        public NewsFilterService(
            ILogger<NewsFilterService> logService,
            IDbContextFactory<NewsContext> dbContextFactory,
            ICurrentUserContext currentUserContext,
            IAuthorizationService authorizationService)
        {
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));
            _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
            _currentUserContext = currentUserContext ?? throw new ArgumentNullException(nameof(currentUserContext));
            _authorizationService = authorizationService ?? throw new ArgumentNullException(nameof(authorizationService));
        }

        public Task<IEnumerable<int>> GetIdFilter()
        {
            return Task.FromResult(Enumerable.Empty<int>());
        }

        public async Task<IQueryable<Models.News>> GetBaseFilter()
        {
            var currentUser = _currentUserContext.Get();
            if (currentUser == null) throw new ArgumentNullException(nameof(currentUser));

            int currentUserId = currentUser.UserId;
            if (currentUserId <= 0) throw new ArgumentOutOfRangeException($"{nameof(currentUserId)}: {currentUserId}");

            var unitPermissions = await _authorizationService.GetUnitPermissions(currentUserId, null, PermissionType.LesNyheter, PermissionType.SkrivNyheter);
            var editorOrgUnitIds = unitPermissions
                .Where(x => x.PermissionType == PermissionType.SkrivNyheter)
                .Select(x => x.UnitId)
                .ToList();
            var readerOrgUnitIds = unitPermissions
                .Select(x => x.UnitId)
                .ToList(); // You have read if you have write
            var onlyReaderOrgUnitIds = readerOrgUnitIds
                .Except(editorOrgUnitIds)
                .ToList();
            
            var hasEditorPermissionOnMaster = editorOrgUnitIds.Contains(UnitConstants.MasterUnitId);
            var hasReadOnlyOnMaster = onlyReaderOrgUnitIds.Contains(UnitConstants.MasterUnitId);

            var ctx = await _dbContextFactory.CreateDbContext();

            var query = (from item in ctx.Set<Models.News>()
                         where (
                                 (!item.UnitId.HasValue && hasEditorPermissionOnMaster) ||
                                 (item.UnitId.HasValue && editorOrgUnitIds.Contains(item.UnitId.Value))
                             )
                             ||
                             (
                               (
                                   (!item.UnitId.HasValue && hasReadOnlyOnMaster) ||
                                   (item.UnitId.HasValue && onlyReaderOrgUnitIds.Contains(item.UnitId.Value))
                               )
                               &&
                               (
                                  
                                  (!item.FromDate.HasValue || item.FromDate.Value.Date <= DateTime.Today) && 
                                  (!item.ToDate.HasValue || item.ToDate.Value.Date >= DateTime.Today)
                               )
                           )
                         select item);

            return query;
        }
    }
}
