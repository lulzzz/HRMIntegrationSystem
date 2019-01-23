using Common.Api.Contracts.Users;
using System.Collections.Generic;
using System.Threading.Tasks;
using contracts = Common.Api.Contracts.Users;
using domain = Common.Api.Domain.Interfaces;

namespace Common.Api.Domain.Services.Proxy
{
    public class UserProxyService : contracts.IUserService
    {
        private readonly domain.IUserService _userService;

        public UserProxyService(domain.IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IUser> GetUser(int id)
        {
            return await _userService.GetById(id);
        }

        public async Task<IEnumerable<IUser>> SearchUser(ISearchQueryUser searchQuery)
        {
            var query = (domain.Users.ISearchQueryUser)searchQuery;
            return await _userService.SearchUser(query);
        }
    }
}
