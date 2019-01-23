using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Text;
using Timereg.Api.Domain.Interfaces;
using Timereg.Api.Domain.Models;
using Timereg.Api.Services.Services;

namespace Timereg.Api.UnitTests.Builders
{
    public class ExternalSystemServiceBuilder
    {
        private IExternalSystemService _externalSystemService = A.Fake<IExternalSystemService>();
        private IExternalSystemRepository _repository = A.Fake<IExternalSystemRepository>();
        private IExternalSystemFactory _externalSystemFactory = A.Fake<IExternalSystemFactory>();

        public ExternalSystemServiceBuilder WithExternalSystemRepository(IExternalSystemRepository repository)
        {
            _repository = repository;
            return this;
        }

        public ExternalSystemServiceBuilder WithExternalSystemFactory(IExternalSystemFactory externalSystemFactory)
        {
            _externalSystemFactory = externalSystemFactory;
            return this;
        }
        public IExternalSystemService Build()
        {
            return new ExternalSystemService(_repository, _externalSystemFactory);
        }
    }
}
