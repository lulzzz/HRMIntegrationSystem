using AutoMapper;
using Common.Api.Contracts.Employees;

namespace Common.Api.Mapping
{
    public class EmployeeMapperProfile : Profile
    {
        public EmployeeMapperProfile()
        {
            // Mapping contracts to domain
            CreateMap<SearchQueryEmployee, Domain.Entities.SearchQueryEmployee>();
        }
    }
}