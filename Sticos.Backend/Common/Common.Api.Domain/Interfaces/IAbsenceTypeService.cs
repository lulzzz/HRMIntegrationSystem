using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Api.Domain.Entities;

namespace Common.Api.Domain.Interfaces
{
    public interface IAbsenceTypeService
    {
        Task<IEnumerable<AbsenceType>> GetAbsenceTypes(SearchQueryAbsenceType query);
    }
}
