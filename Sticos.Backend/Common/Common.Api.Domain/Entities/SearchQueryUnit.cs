using System.Collections.Generic;
using Shared.Domain.Enums;

namespace Common.Api.Domain.Entities
{
    public class SearchQueryUnit : Shared.Contracts.SearchQueryBase
    {
        public List<int> UnitIds { get; set; } = new List<int>();
        public List<int> UnitTypes { get; set; } = new List<int>();

    }
}