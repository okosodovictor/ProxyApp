using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Proxy.Domain.Exceptions;
using Proxy.Domain.Interface.Managers;
using Proxy.Domain.Models;
using Proxy.Web.Controllers;
using Proxy.Web.Interface.Services;
using Proxy.Web.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proxy.Tests.Controllers
{
    
    public class AccountControllerTest
    {
        [Test]
        public void AuthenticateReturnsForbiddenForInvalidRegistration()
        {
            //Arrange
            var mockAccountManager = new Mock<IAccountManager>();
            mockAccountManager.Setup(d => d.ValidateUser(It.IsAny<string>(), It.IsAny<string>())).Throws(new ForbiddenException("Could not find user"));

            var mockLogger = new Mock<ILogger<AccountController>>();
            var mockTokenService = new Mock<ITokenService>();
            var controller = new AccountController(mockAccountManager.Object,  mockTokenService.Object, mockLogger.Object);
            var fakeUser = new User
            {
                Username = "test-proxy_fake",
                Password = "Fake-Password"
            };
            //Act
            var result = controller.Authenticate(fakeUser);
            //Assert
            Assert.IsInstanceOf(typeof(ForbidResult),result.Result);
        }

        [Test]
        public void AuthenticateReturnsTokenForValidRegistration()
        {
            //Arrange
            var fakeUser = new User
            {
                Username = "test-proxy",
                Password = "Password"
            };

            var fakeToken = new TokenModel
            {
                Token = "fake-token",
                DateCreated = DateTime.Now
            };

            var mockLogger = new Mock<ILogger<AccountController>>();
            var mockAccountManager = new Mock<IAccountManager>();
            mockAccountManager.Setup(d => d.ValidateUser(fakeUser.Username, fakeUser.Password)).Returns(fakeUser);

            var mockTokenService = new Mock<ITokenService>();
            mockTokenService.Setup(t => t.GenerateToken(fakeUser)).Returns(fakeToken);

            var controller = new AccountController(mockAccountManager.Object, mockTokenService.Object, mockLogger.Object);
            //Act
            var result = controller.Authenticate(fakeUser);
            //Assert
            Assert.AreEqual(result.Value, fakeToken);
        }
    }
}
