using Altinn.Api.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Altinn.Api.Client.Adapters
{
    public interface IAltinnAdapter
    {
        Task<IEnumerable<NavMessage>> ReadMessages(string externalCompanyId);
        Task<IEnumerable<NavMessage>> WriteMessages(string externalCompanyId, IEnumerable<NavMessage> navMessages);
    }
}