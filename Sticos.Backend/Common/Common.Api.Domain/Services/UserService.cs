using Common.Api.Domain.Interfaces;
using Common.Api.Domain.Interfaces.Users;
using Shared.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Api.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUserContext _currentUserContext;

        public UserService(IUserRepository userRepository, ICurrentUserContext currentUserContext)
        {
            _userRepository = userRepository;
            _currentUserContext = currentUserContext;
        }

        public async Task<IUser> CurrentUser()
        {
            var currentUserContext = _currentUserContext.Get();
            var currentUser = await _userRepository.Get(currentUserContext.UserId);
            return currentUser;
        }

        public async Task<IUser> GetById(int userId)
        {
            return await _userRepository.Get(userId);
        }

        public async Task<IEnumerable<IUser>> SearchUser(ISearchQueryUser searchQuery)
        {
            return await _userRepository.Search(searchQuery);
        }
    }
}
