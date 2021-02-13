﻿using Proxy.Domain.Interface.Repository;
using Proxy.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proxy.Infrastructure.Repositories
{
    public class InMemoryUserRepository : IInMemoryUserRepository
    {
        //This could have be database context injected.
        //This In-Memory user could be query from database or any data store.
        private IEnumerable<User> _inMemoryUsers;

        public InMemoryUserRepository()
        {
            _inMemoryUsers = new List<User>()
            {
                new User{ Id = 1, Username="test-proxy", Password="password"},
                new User{ Id = 1, Username="test-proxy2", Password="password2"}
                //...........
                ///........
            };
        }

        public User GetUser(string username, string password)
        {
            var user = _inMemoryUsers.FirstOrDefault(u => u.Username == username && u.Password == password);
            return user;
        }
    }
}
