using System;
using Shared.Contracts;

namespace Integrations.Api.Domain.Models
{
    public class IntegrationCategory : ICode
    {
        public Guid Id { get; set; }
        public string Type { get; } = typeof(IntegrationCategoryEnum).Name;
        public string Value { get; private set; }
        public int Order { get; set; }
        public string Image { get; set; }

        public IntegrationCategoryEnum SpecificValue
        {
            get => (IntegrationCategoryEnum)Enum.Parse(typeof(IntegrationCategoryEnum), Value);
            set => Value = ((int)value).ToString();
        }
    }
    
}
