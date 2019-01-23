using System;
using Shared.Contracts;

namespace Altinn.Api.Domain.Entities
{
    public class ExternalSystem : ICode
    {
        public Guid Id { get; set; }
        public string Type { get; } = typeof(ExternalGovernmentSystem).Name;
        public string Value { get; private set; }
        public int Order { get; set; }
        public string Image { get; set; }

        public ExternalGovernmentSystem SpecificValue
        {
            get => (ExternalGovernmentSystem)Enum.Parse(typeof(ExternalGovernmentSystem), Value);
            set => Value = ((int)value).ToString();
        }
    }
}