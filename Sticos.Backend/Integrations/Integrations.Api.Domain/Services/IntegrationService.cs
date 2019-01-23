using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.Exceptions;
using Integrations.Api.Domain.Models;
using Integrations.Api.Domain.Validators.Interfaces;
using Integrations.Api.Domain.Interfaces;
using Shared.MessageBus.Contracts;
using Shared.Interfaces;

namespace Integrations.Api.Services.Services
{
    public class IntegrationService : IIntegrationService
    {
        private readonly IRepository<Integration, SearchQueryIntegration> _repository;
        private readonly IEntityValidator<Integration, int?> _validator;
        private readonly IPublisher<IIntegrationDeleted> _integrationDeletePublisher;
        private readonly ICustomerIdService _customerIdService;

        public IntegrationService(IRepository<Integration, SearchQueryIntegration> repository,
                                  IEntityValidator<Integration, int?> validator,
                                  IPublisher<IIntegrationDeleted> integrationDeletePublisher,
                                  ICustomerIdService customerIdService)
        {
            _repository = repository;
            _validator = validator;
            _integrationDeletePublisher = integrationDeletePublisher;
            _customerIdService = customerIdService;
        }

        public async Task<Integration> CreateIntegration(Integration integration)
        {
            var validation = _validator.ValidateCreate(integration);

            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            var searchQuery = new SearchQueryIntegration(integration.UnitId, integration.Category, integration.ExternalSystem);

            var existIntegrations = await Search(searchQuery);

            if (existIntegrations.Any())
            {
                throw new ValidationException("Integration for this company and category already exist");
            }

            var createdIntegration = await _repository.Create(integration);
            return createdIntegration;
        }

        public async Task<IEnumerable<Integration>> Search(SearchQueryIntegration searchQuery)
        {
            var integrations = await _repository.Search(searchQuery);
            return integrations;
        }

        public async Task<Integration> GetIntegration(int id)
        {
            var integration = await _repository.GetSingle(id);
            return integration;
        }

        public async Task<Integration> UpdateIntegration(Integration integration)
        {
            var updatedIntegration = await _repository.Update(integration);
            return updatedIntegration;
        }

        public async Task Delete(int id)
        {
            var integration = await _repository.Delete(id);
            if (integration != null)
            {
                await _integrationDeletePublisher.Publish(new
                {
                    integration.Id,
                    integration.UnitId,
                    integration.ExternalSystem,
                    integration.Category,
                    CustomerId = _customerIdService.GetCustomerIdNotNull()
                });
            }
        }
    }
}
