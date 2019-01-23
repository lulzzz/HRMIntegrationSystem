using System.Collections.Generic;

namespace Shared.Contracts.Charts
{
    public interface IChart
    {
        IEnumerable<IChartSerie> Series { get; set; }
    }
}
