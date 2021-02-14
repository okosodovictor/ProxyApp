using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proxy.Domain.Models
{
    public class RecordRequest
    {
        [JsonProperty("fields")]
        public Field Field { get; set; }
    }
}
