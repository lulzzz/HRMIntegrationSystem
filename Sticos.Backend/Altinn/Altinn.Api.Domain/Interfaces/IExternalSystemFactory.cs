using Altinn.Api.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Altinn.Api.Domain.Interfaces
{
    public interface IExternalSystemFactory
    {
        IExternalDataService CreateDataService(ExternalGovernmentSystem externalSystem);
    }
}
