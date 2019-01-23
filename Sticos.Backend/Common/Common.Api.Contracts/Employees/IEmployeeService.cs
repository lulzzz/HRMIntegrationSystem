using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Api.Contracts.Employees
{
    public interface IEmployeeService
    {
        Task<IEnumerable<IEmployee>> SearchEmployee(ISearchQueryEmployee query);
    }
}