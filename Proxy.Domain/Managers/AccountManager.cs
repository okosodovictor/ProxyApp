using Proxy.Domain.Exceptions;
using Proxy.Domain.Interface.Managers;
using Proxy.Domain.Interface.Repository;
using Proxy.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proxy.Domain.Managers
{
    public class AccountManager : IAccountManager
    {
        private readonly IUserRepository _userRepo;

        public AccountManager(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public User ValidateUser(string username, string password)
        {
            var user = _userRepo.GetUser(username, password);
            if (user == null) throw new ForbiddenException("Invalid username or Password");
            return user;
        }
    }
}
