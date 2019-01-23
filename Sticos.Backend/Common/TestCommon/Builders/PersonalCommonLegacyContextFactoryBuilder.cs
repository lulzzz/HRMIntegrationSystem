using Common.Api.Repositories.Legacy.Factories;
using FakeItEasy;
using Microsoft.Extensions.Configuration;

namespace TestCommon.Builders
{
    public class PersonalCommonLegacyContextFactoryBuilder
    {
        private IConfiguration _configuration  = A.Fake<IConfiguration>();

        public PersonalCommonLegacyContextFactoryBuilder SetConnectionstringFormat(string connectionStringFormat)
        {
            var connectionStringSection = A.Fake<IConfigurationSection>();
            A.CallTo(() => connectionStringSection["SticosPersonalCommonLegacy"]).Returns(connectionStringFormat);
            A.CallTo(() =>_configuration.GetSection("ConnectionStrings")).Returns(connectionStringSection);
            
            return this;
        }

        public PersonalCommonLegacyContextFactory Build()
        {
            return new PersonalCommonLegacyContextFactory(_configuration);
        }
    }
}