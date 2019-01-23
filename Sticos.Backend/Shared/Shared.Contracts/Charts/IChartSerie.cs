    using System.Collections.Generic;

namespace Shared.Contracts.Charts
{
    public interface IChartSerie
    {
        string Name { get; set; }
        IEnumerable<IChartValue> Values { get; set; }

    }
}