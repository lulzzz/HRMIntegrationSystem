using Common.Api.Contracts.Users;
using FakeItEasy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Shared.Interfaces.Models;
using Shared.Services.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Shared.TestCommon
{
    public class TestServerBuilder
    {
        private string _environment = "UnitTesting";
        private string _contentRootPath = Directory.GetCurrentDirectory();
        private Action<IServiceCollection> _postConfigureServiceCollection = (sc) => { };
        private Dictionary<string, string> _configSettings = new Dictionary<string, string>();
        private User _user;

        public TestServerBuilder WithEnvironment(string environment)
        {
            _environment = environment;
            return this;
        }

        public TestServerBuilder WithPostConfigureCollection(Action<IServiceCollection> postConfigureServiceCollection)
        {
            _postConfigureServiceCollection = postConfigureServiceCollection;
            return this;
        }

        public TestServerBuilder WithConfigSettings(Dictionary<string, string> configSettings)
        {
            _configSettings = configSettings;
            return this;
        }
        public TestServerBuilder WithContentRootPath(string contentRootPath)
        {
            _contentRootPath = contentRootPath;
            return this;
        }

        public TestServerBuilder WithCurrentUser(int userId, int customerId, bool isPersonalCustomerAdmin)
        {
            _user = new User()
            {
                CustomerId = customerId,
                IsPersonalCustomerAdmin = isPersonalCustomerAdmin,
                UserId = userId
            };
            return this;
        }

        public TestServer Build<TStartUp>() where TStartUp : class, IConfigurableStartUp
        {
            var hostingEnvironment = A.Fake<IHostingEnvironment>();
            A.CallTo(() => hostingEnvironment.EnvironmentName).Returns(_environment);
            A.CallTo(() => hostingEnvironment.ContentRootPath).Returns(_contentRootPath);

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, false)
                .AddInMemoryCollection(_configSettings)
                .Build();

            var logger = new NullLogger<TStartUp>();
            var myStartup = (TStartUp)Activator.CreateInstance(typeof(TStartUp), logger, hostingEnvironment, configuration);


            myStartup.PostConfigureServiceCollection = _postConfigureServiceCollection;


            var builder = new WebHostBuilder()
                    .ConfigureServices(services =>
                {
                    ConfigureUser(services);
                    services.AddSingleton<IConfiguration>(configuration);
                    services.AddSingleton<IStartup>(myStartup);
                });

            return new TestServer(builder);

        }

        private void ConfigureUser(IServiceCollection services)
        {
            if (_user == null) return;
            var userServiceFake = A.Fake<IUserService>();
            A.CallTo(() => userServiceFake.GetUser(_user.UserId.Value)).Returns(Task.FromResult<IUser>(_user));
            services.ReplaceSingleton(userServiceFake);
        }

    }
}