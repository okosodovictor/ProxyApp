using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proxy.Domain.Models
{
    public class Field
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("Summary")]
        public string Summary { get; set; }
        [JsonProperty("Message")]
        public string Message { get; set; }
        [JsonProperty("receivedAt")]
        public DateTimeOffset ReceivedAt { get; set; }
    }
}
