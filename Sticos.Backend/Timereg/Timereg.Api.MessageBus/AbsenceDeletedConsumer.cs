﻿using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Interfaces;
using Sticos.Personal.MessageContracts;
using System.Threading.Tasks;
using Timereg.Api.Domain.Interfaces;

namespace Timereg.Api.MessageBus
{
    public class AbsenceDeletedConsumer : IConsumer<IAbsenceDeleted>
    {
        private readonly IAbsenceService _absenceService;
        private readonly ILogger<AbsenceApprovedConsumer> _logger;
        private readonly IStaticCustomerId _staticCustomerId;

        public AbsenceDeletedConsumer(IStaticCustomerId staticCustomerId, IAbsenceService absenceService, ILogger<AbsenceApprovedConsumer> logger)
        {
            _absenceService = absenceService;
            _logger = logger;
            _staticCustomerId = staticCustomerId;
        }
        public async Task Consume(ConsumeContext<IAbsenceDeleted> context)
        {
            _logger.LogInformation($"Received AbsenceApprovedMessage with absenceId: {context.Message.AbsenceId}");
            _staticCustomerId.CustomerId = context.Message.CustomerId;

            var absence = MessageToAbsenceMapper.CreateAbsenceFromMessage(context.Message);

            await _absenceService.DeleteAbsence(absence);
        }
    }
}
