using System;
using System.Collections.Generic;
using System.Text;

namespace Absence.Api.Tests.ControllerTests.StatisticsController.Models
{
    public class ChartData
    {
        public List<ChartSerie> Series { get; set; }
    }
    public class ChartSerie
    {
        public string Name { get; set; }
        public List<ChartValue> Values { get; set; }
    }
    public class ChartValue
    {
        public string Name { get; set; }
        public double Value { get; set; }
    }
}
