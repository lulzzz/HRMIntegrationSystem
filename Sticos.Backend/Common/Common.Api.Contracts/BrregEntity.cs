using System.Collections.Generic;

namespace Common.Api.Contracts
{
    public class BrregEntity
    {
        public BrregEntity()
        {
            Children = new List<BrregEntity>();
        }
        public int OrganizationNumber { get; set; }
        public string Name { get; set; }
        public BrregEntityType Type { get; set; }
        public List<BrregEntity> Children { get; set; }
    }
    public enum BrregEntityType
    {
        Parent = 0,
        Child = 1
    }
}