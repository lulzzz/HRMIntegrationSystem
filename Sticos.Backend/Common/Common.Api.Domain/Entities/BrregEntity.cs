using System.Collections.Generic;

namespace Common.Api.Domain.Entities
{
    public class BrregEntity
    {
        public BrregEntity()
        {
            Children = new List<BrregEntity>();
        }
        public int OrganizationNumber { get; set; }
        public string Name { get; set; }
        public List<BrregEntity> Children { get; set; }

        /// <summary>
        /// Default = 1
        /// </summary>
        public BrregEntityType Type { get; set; }
    }

    public enum BrregEntityType
    {
        Parent = 0,
        Child = 1
    }
}