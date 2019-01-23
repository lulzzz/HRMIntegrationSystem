using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Api.Domain.Entities;

namespace Common.Api.Domain.Interfaces
{
    public interface IAnomalyService
    {
        List<Anomaly> Anomalies { get; set; }
        Task<IEnumerable<Anomaly>> SearchAnomaly(SearchQueryAnomaly query);
    }
}