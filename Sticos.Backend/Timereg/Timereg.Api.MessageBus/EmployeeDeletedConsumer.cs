using MassTransit;
using System.Threading.Tasks;
using Sticos.Personal.MessageContracts;
using Timereg.Api.Domain.Interfaces;
using Shared.Interfaces;
using Timereg.Api.Domain.Models;

namespace Timereg.Api.MessageBus
{
    public class EmployeeDeletedConsumer : IConsumer<IEmployeeDeleted>
    {
        private readonly IAbsenceExportService _absenceExportService;
        private readonly IStaticCustomerId _staticCustomerId;
        public EmployeeDeletedConsumer(IAbsenceExportService absenceExportService, IStaticCustomerId staticCustomerId)
        {
            _absenceExportService = absenceExportService;
            _staticCustomerId = staticCustomerId;
        }

        public async Task Consume(ConsumeContext<IEmployeeDeleted> context)
        {
            _staticCustomerId.CustomerId = context.Message.CustomerId;
            var message = context.Message as EmployeeDeleted;
            await _absenceExportService.DeleteEmployee(message);
        }
       
    }
}
