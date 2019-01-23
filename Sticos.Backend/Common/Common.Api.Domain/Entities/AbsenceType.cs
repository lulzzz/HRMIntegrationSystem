using System;
using Shared.Contracts;
using Sticos.Personal.MessageContracts.Enums;

namespace Common.Api.Domain.Entities
{
    public class AbsenceType : ICode
    {
        public Guid Id { get; set; }
        public string Type { get; } = typeof(AbsenceSubType).Name;
        public string Value { get; private set;}
        public string Image { get; set; }

        public AbsenceSubType SpecificValue
        {
            get => (AbsenceSubType) Enum.Parse(typeof(AbsenceSubType), Value);
            set => Value = ((int) value).ToString();
        }

        public int Order { get;set; }
    }
}
