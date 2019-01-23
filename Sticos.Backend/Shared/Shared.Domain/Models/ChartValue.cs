
using Shared.Domain.Interfaces;

namespace Shared.Domain.Models
{
    public class ChartValue : IChartValue
        {
            public string Name { get; set; }
            public double Value { get; set; }
        }
}
