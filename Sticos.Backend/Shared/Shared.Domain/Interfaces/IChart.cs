using System;
using System.Collections.Generic;

namespace Shared.Domain.Interfaces
{
    public interface IChart
    {
        IEnumerable<IChartSerie> Series { get; set; }
    }
}
