using System;
using System.Collections.Generic;

namespace Shared.Domain.Interfaces
{ 
    public interface IChartSerie
    {
        string Name { get; set; }
        IEnumerable<IChartValue> Values { get; set; }
    }
}
