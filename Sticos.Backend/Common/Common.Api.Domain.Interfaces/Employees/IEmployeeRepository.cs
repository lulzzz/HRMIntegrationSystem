using Common.Api.Domain.Interfaces.Repositories;

namespace Common.Api.Domain.Interfaces.Employees
{
    public interface IEmployeeRepository :  ISearchRepository<IEmployee,ISearchQueryEmployee>
    {
    }
}