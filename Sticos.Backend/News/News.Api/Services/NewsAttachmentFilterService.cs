using Microsoft.Extensions.Logging;
using News.Api.Models;
using Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace News.Api.Services
{
    public class NewsAttachmentFilterService : IEntityFilterService<Models.NewsAttachment, int>
    {
        private readonly ILogger<NewsAttachmentFilterService> _logService;

        public NewsAttachmentFilterService(
            ILogger<NewsAttachmentFilterService> logService)
        {
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));
        }

        public Task<IQueryable<NewsAttachment>> GetBaseFilter()
        {
            IQueryable<NewsAttachment> list = null;

            return Task.FromResult(list);
        }

        public Task<IEnumerable<int>> GetIdFilter()
        {
            return Task.FromResult(Enumerable.Empty<int>());
        }
    }
}
