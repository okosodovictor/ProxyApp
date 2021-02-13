using Proxy.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proxy.Domain.Interface.Managers
{
    public interface IAccountManager
    {
        User ValidateUser(string username, string password);
    }
}
