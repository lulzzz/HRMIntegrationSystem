using System;
using Timereg.Api.Domain.Interfaces;
using Timereg.Api.Domain.Models;
using Timereg.Api.Unimicro.Adapters;

namespace Timereg.Api.Services.Services
{
    public class ExternalSystemFactory : IExternalSystemFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ExternalSystemFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IExternalDataService CreateDataService(ExternalEconomySystem externalSystem)
        {
            switch (externalSystem)
            {
                case ExternalEconomySystem.UniMicro:
                    return (IExternalDataService)_serviceProvider.GetService(typeof(UniMicroExternalDataService));
                default:
                    throw new ArgumentOutOfRangeException(nameof(externalSystem), externalSystem, null);
            }
        }

        public IExternalSystemAdapter CreateSystemAdapter(ExternalEconomySystem externalSystem)
        {
            switch (externalSystem)
            {
                case ExternalEconomySystem.UniMicro:
                    return (IExternalSystemAdapter)_serviceProvider.GetService(typeof(UniMicroAdapter));
                default:
                    throw new ArgumentOutOfRangeException(nameof(externalSystem), externalSystem, null);
            }
        }

        public IExternalMatchingService CreateMatchingService(ExternalEconomySystem externalSystem)
        {
            switch (externalSystem)
            {
                case ExternalEconomySystem.UniMicro:
                    return (IExternalMatchingService)_serviceProvider.GetService(typeof(UniMicroMatchingService));
                default:
                    throw new ArgumentOutOfRangeException(nameof(externalSystem), externalSystem, null);
            }
        }
    }
}
