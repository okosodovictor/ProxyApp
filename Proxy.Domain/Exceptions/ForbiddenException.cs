using System;
using System.Collections.Generic;
using System.Text;

namespace Proxy.Domain.Exceptions
{
    public class ForbiddenException: ApplicationException
    {
        public ForbiddenException(string message) : base(message)
        {
        }
    }
}
