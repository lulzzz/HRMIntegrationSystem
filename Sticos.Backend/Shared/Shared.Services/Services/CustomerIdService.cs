using Microsoft.AspNetCore.Mvc.Infrastructure;
using Shared.Interfaces;
using System;

namespace Shared.Services.Services
{
    public class CustomerIdService : ICustomerIdService
    {
        private readonly IActionContextAccessor _actionContext;
        private readonly IStaticCustomerId _staticCustomerId;

        public CustomerIdService(IActionContextAccessor actionContext, IStaticCustomerId staticCustomerId)
        {
            _actionContext = actionContext;
            _staticCustomerId = staticCustomerId;
        }

        public int GetCustomerIdNotNull()
        {
            var customerId = GetCustomerId();

            if (!customerId.HasValue)
            {
                throw new ArgumentException("A valid CustomerId not found in route data");
            }

            return customerId.Value;
        }

        public int? GetCustomerId()
        {
            if (_actionContext.ActionContext == null)
            {
                return _staticCustomerId.CustomerId;
            }
            if (!_actionContext.ActionContext.RouteData.Values.TryGetValue("customerId", out var customerIdValue))
            {
                return null;
            }

            var customerIdString = customerIdValue as string;

            if (!int.TryParse(customerIdString, out var customerId) && customerId > 0)
            {
                return null;
            }

            return customerId;
        }
    }

    public class
        StaticCustomerId : IStaticCustomerId
    {
        private readonly IActionContextAccessor _actionContext;

        public StaticCustomerId(IActionContextAccessor actionContext)
        {
            _actionContext = actionContext;
        }

        private int? _customerId;
        public int? CustomerId
        {
            get => _customerId.Value;
            set
            {
                if (_actionContext.ActionContext != null)
                {
                    throw new Exception("Do not use StaticCustomerId when you have an actionContext available. Use the CustomerId service to get it from the route.");
                }
                _customerId = value;
            }

        }
    }
}
