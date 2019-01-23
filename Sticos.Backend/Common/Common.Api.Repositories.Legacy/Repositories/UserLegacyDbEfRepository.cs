using domain = Common.Api.Domain.Interfaces.Users;
using Common.Api.Repositories.Legacy.Context;
using Common.Api.Repositories.Legacy.Models;
using Shared.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Api.Repositories.Legacy.Repositories
{
    public class UserLegacyDbEfRepository : domain.IUserRepository
    {
        private readonly IDbContextFactory<PersonalLegacyContext> _dbContextFactory;
        private readonly IDbContextFactory<PersonalCommonLegacyContext> _dbCommonContextFactory;

        public UserLegacyDbEfRepository(IDbContextFactory<PersonalCommonLegacyContext> dbCommonContextFactory, IDbContextFactory<PersonalLegacyContext> dbContextFactory)
        {
            _dbCommonContextFactory = dbCommonContextFactory;
            _dbContextFactory = dbContextFactory;
        }

        public async Task<domain.IUser> Get(int userId)
        {
            using (var context = await _dbCommonContextFactory.CreateDbContext())
            {
                var users = (from u in context.Users
                             join c in context.Customers on u.CustomerId equals c.Id into ucjoin
                             from uc in ucjoin.DefaultIfEmpty()
                             where u.UserId == userId
                             select new User
                             {
                                 Id = u.Id,
                                 UserId = u.UserId,
                                 UnitId = u.UnitId,
                                 CustomerId = uc.SticosCustomerId,
                                 Email = u.Email,
                                 Mobilephone = u.Mobilephone,
                                 Workphone = u.Workphone,
                                 FirstName = u.FirstName,
                                 LastName = u.LastName
                             }).ToList();
               
                AttachPropertiesFromEmployeePermissions(users);
                return users.FirstOrDefault();
            }
        }

        public async Task<IList<domain.IUser>> Search(domain.ISearchQueryUser searchQuery)
        {
            using (var context = await _dbCommonContextFactory.CreateDbContext())
            {
                var query = context.Users.Where(x => !x.IsDeleted);
                query = ApplyQueryParameters(searchQuery, query);
                    
                var users = query
                    .OrderBy(x => x.UserId)
                    .Skip(searchQuery.Skip ?? SearchConstants.DEFAULT_SKIP)
                    .Take(searchQuery.Take ?? SearchConstants.DEFAULT_TAKE)
                    .ToList();

                await AttachPropertiesFromEmployeePermissions(users);

                return users.ToList<domain.IUser>();
            }
        }

        private IQueryable<User> ApplyQueryParameters(domain.ISearchQueryUser searchQuery, IQueryable<User> query)
        {
            if (searchQuery.UnitId.HasValue)
            {
                query = query.Where(x => x.UnitId == searchQuery.UnitId.Value);
            }

            return query;
        }

        private async Task AttachPropertiesFromEmployeePermissions(List<User> users)
        {
            int customerAdminPermissionType = 2;
            var userDictionary = users.ToDictionary(u => u.UserId, u => u);
            var userIds = userDictionary.Keys;

            if (userIds.Any())
            {
                using (var context = await _dbContextFactory.CreateDbContext())
                {
                    //todo: possible more than 2000 userids. add batch-logic
                    var permissionDictionary = context.EmployeePermissions
                        .Where(u => userIds.Contains(u.ResponsibleUserId) && u.PermissionType==customerAdminPermissionType)
                        .ToDictionary(u => u.ResponsibleUserId, u => u);
                    foreach (var user in permissionDictionary)
                    {
                        userDictionary[user.Key].IsPersonalCustomerAdmin = true;
                    }
                }
            }
        }
    }
}
