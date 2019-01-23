using Shared.Interfaces;
using Shared.Interfaces.Models;

namespace Shared.Services
{
    public class StaticUserContext : ICurrentUserContext
    {
        private IUserContext _currentUser;

        public StaticUserContext(IUserContext currentUser)
        {
            _currentUser = currentUser;
        }
        public void Set(IUserContext currentUser)
        {
            _currentUser = currentUser;
        }
        public IUserContext Get()
        {
            return _currentUser;
        }
    }
}
