using Common.Api.Domain.Interfaces;
using Common.Api.Domain.Interfaces.Employees;
using Common.Api.Domain.Interfaces.Repositories;
using Common.Api.Domain.Services;
using FakeItEasy;

namespace TestCommon.Builders
{
    public class EmployeeServiceBuilder
    {
        private IEmployeeRepository _repository = A.Fake<IEmployeeRepository>();
        

        public EmployeeServiceBuilder WithRepository(IEmployeeRepository repository)
        {
            _repository = repository;
            return this;
        }

        public IEmployeeService Build()
        {
            return new EmployeeService(_repository);
        }
 
    }
}