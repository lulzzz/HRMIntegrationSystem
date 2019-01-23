using Common.Api.Domain.Entities;
using Common.Api.Domain.Interfaces;
using Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Api.Domain.Services
{
    public class WidgetMockDataService : IAnomalyService, INotificationService
    {
        public WidgetMockDataService()
        {
            Anomalies = new List<Anomaly>
            {
                new Anomaly
                {
                    Id = 1,
                    Date = new DateTimeOffset(DateTime.Today),
                    Location = "Toalett",
                    Description = "Description of something",
                    Responsible = "Tarik Eminagic",
                    Deadline = new DateTimeOffset(DateTime.Today),
                    Status = "Close"
                },
                new Anomaly
                {
                    Id = 2,
                    Date = new DateTimeOffset(DateTime.Today),
                    Location = "Toalett",
                    Description = "Description of something",
                    Responsible = "Ensar Karovic",
                    Deadline = new DateTimeOffset(DateTime.Today),
                    Status = "OnGoing"
                },
                new Anomaly
                {
                    Id = 3,
                    Date = new DateTimeOffset(DateTime.Today),
                    Location = "Toalett",
                    Description = "Description of something",
                    Responsible = "Thor Halvor",
                    Deadline = new DateTimeOffset(DateTime.Today),
                    Status = "Close"
                },
                new Anomaly
                {
                    Id = 4,
                    Date = new DateTimeOffset(DateTime.Today),
                    Location = "Toalett",
                    Description = "Description of something",
                    Responsible = "Armin Gerina",
                    Deadline = new DateTimeOffset(DateTime.Today),
                    Status = "Close"
                },
                new Anomaly
                {
                    Id = 5,
                    Date = new DateTimeOffset(DateTime.Today),
                    Location = "Toalett",
                    Description = "Description of something",
                    Responsible = "Selmir Lokmic",
                    Deadline = new DateTimeOffset(DateTime.Today),
                    Status = "OnGoing"
                },
                new Anomaly
                {
                    Id = 6,
                    Date = new DateTimeOffset(DateTime.Today),
                    Location = "Toalett",
                    Description = "Description of something",
                    Responsible = "Kenan Gutic",
                    Deadline = new DateTimeOffset(DateTime.Today),
                    Status = "Close"
                },
                new Anomaly
                {
                    Id = 7,
                    Date = new DateTimeOffset(DateTime.Today),
                    Location = "Toalett",
                    Description = "Description of something",
                    Responsible = "Kenan Gutic",
                    Deadline = new DateTimeOffset(DateTime.Today),
                    Status = "OnGoing"
                },
                new Anomaly
                {
                    Id = 8,
                    Date = new DateTimeOffset(DateTime.Today),
                    Location = "Toalett",
                    Description = "Description of something",
                    Responsible = "Kenan Gutic",
                    Deadline = new DateTimeOffset(DateTime.Today),
                    Status = "OnGoing"
                },
                new Anomaly
                {
                    Id = 9,
                    Date = new DateTimeOffset(DateTime.Today),
                    Location = "Toalett",
                    Description = "Description of something",
                    Responsible = "Thor Halvor",
                    Deadline = new DateTimeOffset(DateTime.Today),
                    Status = "Close"
                },
                new Anomaly
                {
                    Id = 10,
                    Date = new DateTimeOffset(DateTime.Today),
                    Location = "Toalett",
                    Description = "Description of something",
                    Responsible = "Tarik Eminagic",
                    Deadline = new DateTimeOffset(DateTime.Today),
                    Status = "OnGoing"
                },
                new Anomaly
                {
                    Id = 11,
                    Date = new DateTimeOffset(DateTime.Today),
                    Location = "Toalett",
                    Description = "Description of something",
                    Responsible = "Kenan Gutic",
                    Deadline = new DateTimeOffset(DateTime.Today),
                    Status = "Close"
                },
                new Anomaly
                {
                    Id = 12,
                    Date = new DateTimeOffset(DateTime.Today),
                    Location = "Toalett",
                    Description = "Description of something",
                    Responsible = "Ensar Karovic",
                    Deadline = new DateTimeOffset(DateTime.Today),
                    Status = "OnGoing"
                },
                new Anomaly
                {
                    Id = 13,
                    Date = new DateTimeOffset(DateTime.Today),
                    Location = "Toalett",
                    Description = "Description of something",
                    Responsible = "Thor Halvor",
                    Deadline = new DateTimeOffset(DateTime.Today),
                    Status = "Close"
                },
                new Anomaly
                {
                    Id = 14,
                    Date = new DateTimeOffset(DateTime.Today),
                    Location = "Toalett",
                    Description = "Description of something",
                    Responsible = "Ensar Karovic",
                    Deadline = new DateTimeOffset(DateTime.Today),
                    Status = "Close"
                },
                new Anomaly
                {
                    Id = 15,
                    Date = new DateTimeOffset(DateTime.Today),
                    Location = "Toalett",
                    Description = "Description of something",
                    Responsible = "Tarik Eminagic",
                    Deadline = new DateTimeOffset(DateTime.Today),
                    Status = "OnGoing"
                },
                new Anomaly
                {
                    Id = 16,
                    Date = new DateTimeOffset(DateTime.Today),
                    Location = "Toalett",
                    Description = "Description of something",
                    Responsible = "Kenan Gutic",
                    Deadline = new DateTimeOffset(DateTime.Today),
                    Status = "Close"
                },
                new Anomaly
                {
                    Id = 17,
                    Date = new DateTimeOffset(DateTime.Today),
                    Location = "Toalett",
                    Description = "Description of something",
                    Responsible = "Tarik Eminagic",
                    Deadline = new DateTimeOffset(DateTime.Today),
                    Status = "OnGoing"
                },
                new Anomaly
                {
                    Id = 18,
                    Date = new DateTimeOffset(DateTime.Today),
                    Location = "Toalett",
                    Description = "Description of something",
                    Responsible = "Kenan Gutic",
                    Deadline = new DateTimeOffset(DateTime.Today),
                    Status = "OnGoing"
                },
                new Anomaly
                {
                    Id = 19,
                    Date = new DateTimeOffset(DateTime.Today),
                    Location = "Toalett",
                    Description = "Description of something",
                    Responsible = "Thor Halvor",
                    Deadline = new DateTimeOffset(DateTime.Today),
                    Status = "Close"
                },
                new Anomaly
                {
                    Id = 20,
                    Date = new DateTimeOffset(DateTime.Today),
                    Location = "Toalett",
                    Description = "Description of something",
                    Responsible = "Kenan Gutic",
                    Deadline = new DateTimeOffset(DateTime.Today),
                    Status = "OnGoing"
                }
            };

            Notifications = new List<Notification>
            {
                new Notification
                {
                    Id = 1,
                    Title = "Notification 1",
                    Body = "Some message text 1",
                    Date = new DateTimeOffset(DateTime.Today)
                },
                new Notification
                {
                    Id = 2,
                    Title = "Notification 2",
                    Body = "Some message text 2",
                    Date = new DateTimeOffset(DateTime.Today)
                },
                new Notification
                {
                    Id = 3,
                    Title = "Notification 3",
                    Body = "Some message text 3",
                    Date = new DateTimeOffset(DateTime.Today)
                },
                new Notification
                {
                    Id = 4,
                    Title = "Notification 4",
                    Body = "Some message text 4",
                    Date = new DateTimeOffset(DateTime.Today)
                },
                new Notification
                {
                    Id = 5,
                    Title = "Notification 5",
                    Body = "Some message text 5",
                    Date = new DateTimeOffset(DateTime.Today)
                }
            };
        }

        public List<Anomaly> Anomalies { get; set; }

        public async Task<IEnumerable<Anomaly>> SearchAnomaly(SearchQueryAnomaly query)
        {
            var anommalies = Anomalies.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(query.Location))
                anommalies = anommalies.Where(a => a.Location.Contains(query.Location));

            if (!string.IsNullOrWhiteSpace(query.Responsible))
                anommalies = anommalies.Where(a => a.Responsible.ToLower().Contains(query.Responsible.ToLower()));

            anommalies = anommalies
                .Skip(query.Skip ?? SearchConstants.DEFAULT_SKIP)
                .Take(query.Take ?? SearchConstants.DEFAULT_TAKE);
            return anommalies.ToList();
        }


        public List<Notification> Notifications { get; set; }

        public async Task<IEnumerable<Notification>> SearchNotification(SearchQueryNotification query)
        {
            var notifications = Notifications.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(query.Title))
                notifications = notifications.Where(n => n.Title.Contains(query.Title));

            notifications = notifications
                .Skip(query.Skip ?? SearchConstants.DEFAULT_SKIP)
                .Take(query.Take ?? SearchConstants.DEFAULT_TAKE);
            return notifications.ToList();
        }
    }
}
