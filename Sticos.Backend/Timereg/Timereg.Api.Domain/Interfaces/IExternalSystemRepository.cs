using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Timereg.Api.Domain.Models;

namespace Timereg.Api.Domain.Interfaces
{
    public interface IExternalSystemRepository
    {
        Task<List<ExternalSystem>> Search();
        Task<ExternalSystem> GetExternalSystem(int externalSystem);
    }
}
