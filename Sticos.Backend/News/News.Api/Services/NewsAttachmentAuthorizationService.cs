using News.Api.Models;
using Shared.Interfaces;
using System.Threading.Tasks;

namespace News.Api.Services
{
    public class NewsAttachmentAuthorizationService : IEntityAuthorizationService<Models.NewsAttachment, int>
    {
        public async Task<bool> CanCreate(NewsAttachment entity)
        {
            return false;
        }

        public async Task<bool> CanDelete(NewsAttachment entity)
        {
            return false;
        }

        public async Task<bool> CanUpdate(NewsAttachment entity)
        {
            return false;
        }
    }
}
