using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.Interfaces.Queries;
using Timereg.Api.Domain.Interfaces;
using Timereg.Api.Domain.Models;

namespace Timereg.Api.Services.Services
{
    public class AbsenceExportService : IAbsenceExportService
    {
        private readonly IAbsenceExportRepository _repository;
        private readonly IUnitQueries _unitQueries;

        public AbsenceExportService(IAbsenceExportRepository repository, IUnitQueries unitQueries)
        {
            _repository = repository;
            _unitQueries = unitQueries;
        }

        public async Task<AbsenceExport> CreateAbsenceExport(AbsenceExport absenceExport)
        {
            var result = await _repository.Create(absenceExport);
            return result;
        }

        public async Task Delete(int unitId)
        {
            await _repository.Delete(unitId);
        }

        public async Task DeleteEmployee(EmployeeDeleted message)
        {
            await _repository.DeleteEmployee(message);
        }

        public async Task<IEnumerable<AbsenceExport>> Search(SearchQueryAbsenceExport searchQuery)
        {
            var hierarchyDown = await _unitQueries.GetHierarchyDown(searchQuery.UnitId);
            searchQuery.UnitIds = hierarchyDown.Select(u => u.Id).ToList();
            return await _repository.Search(searchQuery);
        }

        public async Task<AbsenceExport> UpdateAbsenceExport(AbsenceExport absenceExport)
        {
            var result = await _repository.Update(absenceExport);
            return result;
        }
    }
}
