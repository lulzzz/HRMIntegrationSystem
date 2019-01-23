using Integrations.Api.Domain.Interfaces;
using Integrations.Api.Domain.Models;
using MassTransit;
using Shared.Interfaces;
using Sticos.Personal.MessageContracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Integrations.Api.MessageBus
{
    public class EmployeeDeletedConsumer : IConsumer<IEmployeeDeleted>
    {
        private readonly IEntityMapService _entityMapService;
        private readonly IStaticCustomerId _staticCustomerId;

        public EmployeeDeletedConsumer(IEntityMapService entityMapService, IStaticCustomerId staticCustomerId)
        {
            _entityMapService = entityMapService;
            _staticCustomerId = staticCustomerId;
        }
        public async Task Consume(ConsumeContext<IEmployeeDeleted> context)
        {
            _staticCustomerId.CustomerId = context.Message.CustomerId;
            var message = context.Message as EmployeeDeleted;
            await _entityMapService.DeleteEmployee(message);
        }
    }
}
