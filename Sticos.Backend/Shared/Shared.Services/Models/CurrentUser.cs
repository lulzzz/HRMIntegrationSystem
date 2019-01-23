using Shared.Interfaces.Models;

namespace Shared.Services.Models
{
    public class UserContext : IUserContext
    {
        public int UserId { get; set; }
    }
}