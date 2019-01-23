using System.Collections.Generic;
using System.Threading.Tasks;

using Altinn.Api.Domain.Entities;

namespace Altinn.Api.Domain.Interfaces
{
    public interface INavMessageService
    {
        Task<IEnumerable<NavMessage>> Search(SearchQueryNavMessage searchParameters);
        void ImportMessages(string businessOrganizationNumber);
        void ExportMessages(string businessOrganizationNumber);
        void ProcessMessages(string businessOrganizationNumber);
    }
}