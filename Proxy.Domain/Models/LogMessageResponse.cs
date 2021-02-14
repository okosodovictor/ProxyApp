using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proxy.Domain.Models
{
    public class LogMessageResponse
    {
        [JsonProperty("records")]
        public RecordResponse[] Records { get; set; }
    }
}
