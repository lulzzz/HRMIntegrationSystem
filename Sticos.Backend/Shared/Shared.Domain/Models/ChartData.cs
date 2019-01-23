using Shared.Domain.Interfaces;
using System.Collections.Generic;


namespace Shared.Domain.Models
{
    public class ChartData : IChart
    {
        public IEnumerable<IChartSerie> Series { get; set; }
    }
}
