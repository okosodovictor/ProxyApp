using Proxy.Domain.Interface.Repository;
using Proxy.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proxy.Infrastructure.Repositories
{
    /// <summary>
    /// This isusually the  database context to be injected here
    /// This In-Memory list of user is just for demonstration purpose purpose
    /// It should be from the database or any file system
    /// </summary>
    public class InMemoryUserRepository : IUserRepository
    {
        private IEnumerable<User> _inMemoryUsers;

        public InMemoryUserRepository()
        {
            _inMemoryUsers = new List<User>()
            {
                new User{ Id = 1, Username="test-proxy", Password="password"},
                new User{ Id = 2, Username="test-proxy2", Password="password2"}
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
