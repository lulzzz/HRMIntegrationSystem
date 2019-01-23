using Shared.Domain.Interfaces;
using System.Collections.Generic;

namespace Shared.Domain.Models
{
    public class ChartSerie : IChartSerie
    {
        public string Name { get; set; }
        public IEnumerable<IChartValue> Values { get; set; }
    }
}