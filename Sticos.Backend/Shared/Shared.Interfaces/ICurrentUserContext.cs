using Shared.Interfaces.Models;

namespace Shared.Interfaces
{
    public interface ICurrentUserContext
    {
        IUserContext Get();
    }
}