using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proxy.Domain.Models
{
    public class LogMessageRequest:Model
    {
        [JsonProperty("records")]
        public RecordRequest[] Records { get; set; }
    }
}
