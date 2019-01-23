using System;

namespace Shared.Contracts
{
    public interface ICode
    {
        Guid Id { get;}
        string Type { get;}
        string Value { get;}
        int Order { get;}
        string Image { get; set; }
    }

    public class Code : ICode
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public int Order { get; set; }
        public string Image { get; set; }
    }
}
