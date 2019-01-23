using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.Contracts;

namespace Common.Api.Contracts.Services
{
    public interface IAbsenceTypeService
    {
        Task<IEnumerable<ICode>> GetAbsenceTypes(SearchQueryAbsenceType query);
    }
}