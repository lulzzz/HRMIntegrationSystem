using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Api.Contracts.Users
{
    public interface IUserService
    {
        Task<IEnumerable<IUser>> SearchUser(ISearchQueryUser searchQuery);
        Task<IUser> GetUser(int id);
    }
}
