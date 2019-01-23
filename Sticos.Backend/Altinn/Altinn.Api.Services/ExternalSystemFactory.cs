using Altinn.Api.Domain.Entities;
using Altinn.Api.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Altinn.Api.Services
{
    public class ExternalSystemFactory : IExternalSystemFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ExternalSystemFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public IExternalDataService CreateDataService(ExternalGovernmentSystem externalSystem)
        {
            switch (externalSystem)
            {
                case ExternalGovernmentSystem.DigiSyfo:
                    return (IExternalDataService)_serviceProvider.GetService(typeof(AltinnExternalDataService));
                default:
                    throw new ArgumentOutOfRangeException(nameof(externalSystem), externalSystem, null);
            }
        }
    }
}
