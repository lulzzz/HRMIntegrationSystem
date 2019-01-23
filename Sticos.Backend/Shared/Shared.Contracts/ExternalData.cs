using System.Collections.Generic;

namespace Shared.Contracts
{
    public class ExternalData
    {
        public List<Identifier> Identifiers { get; } = new List<Identifier>();
        public List<Data> DataSet { get; } = new List<Data>();
        public bool ValidForUse { get; set; }
        public List<string> NotValidReasons { get; set; } = new List<string>();
        public List<string> NotValidReasonsEnums { get; set; } = new List<string>();
    }
}
