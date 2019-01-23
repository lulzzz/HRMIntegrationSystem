using Shared.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Altinn.Api.Domain.Interfaces
{
    public interface IExternalDataService
    {
        Task<IEnumerable<ExternalData>> GetExternalReportees();
    }
}
