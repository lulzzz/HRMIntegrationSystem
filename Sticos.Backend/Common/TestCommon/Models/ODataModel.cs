using Newtonsoft.Json;
using Shared.Domain;
using System.Collections.Generic;

namespace TestCommon.Models
{
    public class ODataModel<T, TId> where T : EntityBase<T, TId>
    {
        [JsonProperty("odata.metadata")]
        public string Metadata { get; set; }
        public T Value { get; set; }
    }

    public class ODataArrayModel<T, TId> where T : EntityBase<T, TId>
    {
        [JsonProperty("odata.metadata")]
        public string Metadata { get; set; }
        public List<T> Value { get; set; }
    }
}
