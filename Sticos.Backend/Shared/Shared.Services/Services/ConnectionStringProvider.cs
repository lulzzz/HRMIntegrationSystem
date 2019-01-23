using System;
using Microsoft.Extensions.Configuration;
using Shared.Interfaces;

namespace Shared.Services.Services
{
    public class ConnectionStringProvider : IConnectionStringProvider
    {
        private readonly ICustomerIdService _customerIdService;
        private readonly IConfiguration _configuration;

        public ConnectionStringProvider(ICustomerIdService customerIdService, IConfiguration configuration)
        {
            _customerIdService = customerIdService ?? throw new ArgumentNullException(nameof(customerIdService));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public string GetForShared()
        {
            var connectionString = _configuration.GetConnectionString("Shared");

            return connectionString;
        }

        public string GetForCustomer()
        {
            var connectionStringTemplate = _configuration.GetConnectionString("Customer");

            var customerId = _customerIdService.GetCustomerIdNotNull();
            var connectionString = string.Format(connectionStringTemplate, customerId);

            return connectionString;
        }
    }
}