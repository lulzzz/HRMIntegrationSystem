using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Api.Domain.Interfaces.Employees;

namespace Common.Api.Domain.Interfaces
{
    public interface IEmployeeService
    {
        Task<IEnumerable<IEmployee>> SearchEmployee(ISearchQueryEmployee query);
    }
}