using Common.Api.Domain.Interfaces.Repositories;
using System.Threading.Tasks;

namespace Common.Api.Domain.Interfaces.Users
{
    public interface IUserRepository: ISearchRepository<IUser, ISearchQueryUser>
    {
        Task<IUser> Get(int id);
    }
}
