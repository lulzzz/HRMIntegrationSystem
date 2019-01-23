
using System.Runtime.Serialization;

namespace Timereg.Api.Unimicro.Models
{
    [DataContract]
    public class Login
    {
        [DataMember(Name ="access_token")]
        public string AccessToken { get; set; }
        [DataMember(Name ="token_type")]
        public string TokenType { get; set; }
        [DataMember(Name = "expires_in")]
        public long ExpiresIn { get; set; }
    }
}
