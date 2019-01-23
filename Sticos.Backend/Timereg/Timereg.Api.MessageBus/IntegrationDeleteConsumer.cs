using MassTransit;
using Shared.Interfaces;
using Shared.MessageBus.Contracts;
using System;
using System.Threading.Tasks;
using Timereg.Api.Domain.Interfaces;

namespace Timereg.Api.MessageBus
{
    public class IntegrationDeleteConsumer : IConsumer<IIntegrationDeleted>
    {
        private readonly IAbsenceExportService _absenceExportService;
        private readonly IStaticCustomerId _staticCustomerId;

        public IntegrationDeleteConsumer(IStaticCustomerId staticCustomerId, IAbsenceExportService absenceExportService)
        {
            _absenceExportService = absenceExportService;
            _staticCustomerId = staticCustomerId;
        }

        public async Task Consume(ConsumeContext<IIntegrationDeleted> context)
        {
            Enum.TryParse(context.Message.Category.ToString(), out IntegrationCategoryEnum category);
            _staticCustomerId.CustomerId = context.Message.CustomerId;

            if (category == IntegrationCategoryEnum.Timereg)
            {
                await _absenceExportService.Delete(context.Message.UnitId);
            }
        }

        private enum IntegrationCategoryEnum
        {
            Unknown = 0,
            Timereg = 1,
            Goverment = 2
        }
    }
}
