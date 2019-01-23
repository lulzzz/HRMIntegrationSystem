using System.Collections.Generic;
using Shared.Contracts;

namespace Common.Api.Contracts
{
    public class SearchQueryUnit : SearchQueryBase
    {
        public List<int> UnitIds { get; set; }
        public List<int> UnitTypes { get; set; } = new List<int>();
    }
}