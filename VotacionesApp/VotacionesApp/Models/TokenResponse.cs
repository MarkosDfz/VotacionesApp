using System;
using Newtonsoft.Json;

namespace VotacionesApp.Models
{
    public class TokenResponse
    {
        [JsonProperty(PropertyName = "access_token")]
        public string Token { get; set; }

        [JsonProperty(PropertyName = ".expires")]
        public DateTime Expiration { get; set; }
    }
}
