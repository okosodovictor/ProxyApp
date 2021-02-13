using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proxy.Web.Models
{
    public class TokenModel
    {
        public string Token { get; set; }
        public DateTimeOffset DateCreated { get; set; }
    }
}
