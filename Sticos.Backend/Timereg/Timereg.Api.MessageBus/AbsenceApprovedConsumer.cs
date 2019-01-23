using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Interfaces;
using Sticos.Personal.MessageContracts;
using System.Threading.Tasks;
using Timereg.Api.Domain.Interfaces;

namespace Timereg.Api.MessageBus
{
    public class AbsenceApprovedConsumer : IConsumer<IAbsenceApproved>
    {
        private readonly IStaticCustomerId _staticCustomerId;
        private readonly IAbsenceService _absenceService;
        private readonly ILogger<AbsenceApprovedConsumer> _logger;

        public AbsenceApprovedConsumer(IStaticCustomerId staticCustomerId, IAbsenceService absenceService, ILogger<AbsenceApprovedConsumer> logger)
        {
            _staticCustomerId = staticCustomerId;
            _absenceService = absenceService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<IAbsenceApproved> context)
        {
            _logger.LogInformation($"Received AbsenceApprovedMessage with absenceId: {context.Message.AbsenceId}");
            _staticCustomerId.CustomerId = context.Message.CustomerId;

            var absence = MessageToAbsenceMapper.CreateAbsenceFromMessage(context.Message);


            await _absenceService.ExportAbsence(absence);
        }
    }
}
