using System;
using Shared.Contracts;

namespace Timereg.Api.Contracts
{
    public class AbsenceType : ICode
    {
        public Guid Id { get; }
        public string Type { get; set; }
        public string Value { get; set; }
        public int Order { get; set; }
        public string Image { get; set; }
    }
}
