using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Api.Domain.Entities;
using Common.Api.Domain.Interfaces;
using Common.Api.Domain.Interfaces.Repositories;
using Common.Api.Domain.Validators.Interfaces;
using Shared.Exceptions;

namespace Common.Api.Domain.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IRepository<Dashboard, SearchQueryDashboard> _dashboardRepository;
        private readonly IEntityValidator<Dashboard, int?> _validator;

        public DashboardService(IRepository<Dashboard, SearchQueryDashboard> dashboardRepository,
            IEntityValidator<Dashboard, int?> validator)
        {
            _dashboardRepository = dashboardRepository;
            _validator = validator;
        }

        public async Task<Dashboard> Create(Dashboard dashboard)
        {
            var validation = _validator.ValidateCreate(dashboard);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            var createdDashboard = await _dashboardRepository.Create(dashboard);
            return createdDashboard;
        }

        public async Task<Dashboard> Update(Dashboard dashboard)
        {
            var validation = _validator.ValidateUpdate(dashboard);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);
            // ReSharper disable once PossibleInvalidOperationException
            await ValidateExistInDb(dashboard.Id.Value);

            var updatedDashboard = await _dashboardRepository.Update(dashboard);
            return updatedDashboard;
        }

        public async Task<Dashboard> Delete(int id)
        {
            var validation = _validator.ValidateDelete(id);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);
            await ValidateExistInDb(id);

            var deletedDashboard = await _dashboardRepository.Delete(id);
            return deletedDashboard;
        }

        public async Task<Dashboard> GetById(int id)
        {
            await ValidateExistInDb(id);

            var dashboard = await _dashboardRepository.GetById(id);
            return dashboard;
        }

        public async Task<IEnumerable<Dashboard>> Search(SearchQueryDashboard query)
        {
            var searchDashboard = await _dashboardRepository.Search(query);
            return searchDashboard;
        }

        private async Task ValidateExistInDb(int id)
        {
            if (!await _dashboardRepository.Exists(id))
                throw new NotFoundException();
        }
    }
}