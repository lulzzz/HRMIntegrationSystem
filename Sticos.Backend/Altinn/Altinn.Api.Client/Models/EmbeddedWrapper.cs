using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Altinn.Api.Client.Models
{
    public class Message
    {
        public string MessageId { get; set; }
        public string Subject { get; set; }
    }
    public class Attachment
    {
        public string FileName { get; set; }
        public string AttachmentId => Links?.Self?.Href?.Split('/')?.LastOrDefault();

        [JsonProperty("_Links")]
        public Links Links { get; set; }
    }

    public class Links
    {
        public Link Self { get; set; }
    }
    public class Link
    {
        public string Href { get; set; }
    }

    public class EmbeddedWrapper<T>
    {
        [JsonProperty("_Embedded")]
        public T Embedded { get; set; }
    }

    public abstract class ItemWrapper<T>
    {
        public abstract IEnumerable<T> Items { get; set; }
    }

    public class MessageWrapper<T> : ItemWrapper<T>
    {
        [JsonProperty("messages")]
        public override IEnumerable<T> Items { get; set; }
    }
    public class AttachmentWrapper<T> : ItemWrapper<T>
    {
        [JsonProperty("attachments")]
        public override IEnumerable<T> Items { get; set; }
    }
    public class ReporteeWrapper<T> : ItemWrapper<T>
    {
        [JsonProperty("reportees")]
        public override IEnumerable<T> Items { get; set; }
    }
}
