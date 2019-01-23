using Common.Api.Repositories.Legacy.Factories;
using FakeItEasy;
using Microsoft.Extensions.Configuration;
using Shared.Interfaces;

namespace TestCommon.Builders
{
    public class PersonalLegacyContextFactoryBuilder
    {
        private IConfiguration _configuration = A.Fake<IConfiguration>();
        private ICustomerIdService _customerIdService = A.Fake<ICustomerIdService>();

        public PersonalLegacyContextFactoryBuilder()
        {
        }
        public PersonalLegacyContextFactoryBuilder WithCurrentCustomerId(int customerId)
        {
            A.CallTo(() => _customerIdService.GetCustomerIdNotNull()).Returns(customerId);
            return this;
        }

        public PersonalLegacyContextFactoryBuilder SetConnectionstringFormat(string connectionStringFormat)
        {
            var connectionStringSection = A.Fake<IConfigurationSection>();
            A.CallTo(() => connectionStringSection["SticosPersonalLegacy"]).Returns(connectionStringFormat);
            A.CallTo(() => _configuration.GetSection("ConnectionStrings")).Returns(connectionStringSection);

            return this;
        }

        public PersonalLegacyContextFactory Build()
        {
            return new PersonalLegacyContextFactory(_configuration, _customerIdService);
        }
    }
}