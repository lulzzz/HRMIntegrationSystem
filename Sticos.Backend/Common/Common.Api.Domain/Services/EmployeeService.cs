using Common.Api.Domain.Interfaces;
using Common.Api.Domain.Interfaces.Employees;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Api.Domain.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repository;

        public EmployeeService(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<IEmployee>> SearchEmployee(ISearchQueryEmployee query)
        {
            return await _repository.Search(query);
        }
    }
}