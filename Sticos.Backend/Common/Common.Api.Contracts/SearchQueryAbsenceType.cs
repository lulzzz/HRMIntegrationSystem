using Shared.Contracts;
using System.Collections.Generic;

namespace Common.Api.Contracts
{
    public class SearchQueryAbsenceType : SearchQueryBase
    {
        public List<int> AbsenceTypesIds { get; set; }
    }
}