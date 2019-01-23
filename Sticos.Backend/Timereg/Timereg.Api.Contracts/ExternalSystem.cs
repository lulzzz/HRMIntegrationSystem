using System;
using System.Collections.Generic;
using System.Text;

namespace Timereg.Api.Contracts
{
    public class ExternalSystem
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public int Order { get; set; }
        public string Image { get; set; }
        public string WebsiteUrl { get; set; }
    }
}
