using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proxy.Domain.Models
{
    public class RecordResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("fields")]
        public Field Field { get; set; }
        [JsonProperty("createdTime")]
        public DateTime CreatedTime { get; set; }
    }
}
