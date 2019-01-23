using Common.Api.Contracts;
using Common.Api.Contracts.Employees;

namespace Altinn.Api.Domain.Interfaces
{
    public interface IAbsenceService
    {
        void CreateAbsence(object sm, IEmployee employee);
    }
}