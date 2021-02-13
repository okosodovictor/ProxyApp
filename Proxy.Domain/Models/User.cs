using System;
using System.Collections.Generic;
using System.Text;

namespace Proxy.Domain.Models
{
    public class User:Model
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
