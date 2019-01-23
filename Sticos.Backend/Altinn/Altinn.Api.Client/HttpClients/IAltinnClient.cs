using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using Altinn.Api.Client.Models;

namespace Altinn.Api.Client.HttpClients
{
    public interface IAltinnClient
    {
        Task<IEnumerable<Message>> GetMessages(string reporteeId);
        Task<IEnumerable<Attachment>> GetAttachments(string reporteeId, string messageId);
        Task<XDocument> GetAttachmentData(string reporteeId, string messageId, string attachmentId);
        Task<IEnumerable<Reportee>> GetReportees();
    }
}

