using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Api.Domain.Entities;

namespace Common.Api.Domain.Interfaces
{
    public interface INotificationService
    {
        List<Notification> Notifications { get; set; }
        Task<IEnumerable<Notification>> SearchNotification(SearchQueryNotification query);
    }
}