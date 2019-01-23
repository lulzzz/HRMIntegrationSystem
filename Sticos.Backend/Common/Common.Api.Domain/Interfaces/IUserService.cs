using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Api.Domain.Interfaces.Users;

namespace Common.Api.Domain.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<IUser>> SearchUser(ISearchQueryUser searchQuery);
        Task<IUser> CurrentUser();
        Task<IUser> GetById(int userId);
    }
}