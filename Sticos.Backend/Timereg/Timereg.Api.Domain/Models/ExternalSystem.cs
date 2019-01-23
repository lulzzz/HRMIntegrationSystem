using System;
using Shared.Contracts;

namespace Timereg.Api.Domain.Models
{
    public class ExternalSystem : ICode
    {
        public Guid Id { get; set; }

        public string Type { get; } = typeof(ExternalEconomySystem).Name;

        public string Value { get; private set; }
        
        public int Order { get; set; }

        public string Image { get; set; }

        public ExternalEconomySystem SpecificValue
        {
            get => (ExternalEconomySystem)Enum.Parse(typeof(ExternalEconomySystem), Value);
            set => Value = ((int)value).ToString();
        }
        public string WebsiteUrl { get; set; }
    }

    public enum ExternalEconomySystem
    {
        Unknown = 0,
        // Xledger = 1,
        UniMicro = 2,
        // Unit4 = 3,
    }
}
