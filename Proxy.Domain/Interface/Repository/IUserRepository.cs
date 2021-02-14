using Proxy.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proxy.Domain.Interface.Repository
{
    public interface IUserRepository
    {
        User GetUser(string username, string password);
    }
}
