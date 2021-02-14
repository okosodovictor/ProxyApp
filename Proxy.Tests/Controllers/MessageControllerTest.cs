using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using Proxy.Domain.Interface.Managers;
using Proxy.Domain.Models;
using Microsoft.Extensions.Logging;
using Proxy.Web.Controllers.API;
using Proxy.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Proxy.Tests.Controllers
{
    public class MessageControllerTest
    {
        [Test]
        public async Task PostReturnsBadRequestForInvalidMessages()
        {
            //Arrange
            var mockLogMessageManager = new Mock<ILogMessageManager>();
            mockLogMessageManager.Setup(x=>x.SendLogMessageAsync(It.IsAny<Message[]>())).Throws(new InvalidOperationException("Invalid message to send"));

            var mockLogger = new Mock<ILogger<MessageController>>();
            var controller = new MessageController(mockLogMessageManager.Object, mockLogger.Object);           
            var fakeMessages = new LogMessageModel()
            {
                 Messages = Array.Empty<Message>()
            };

            //Act
            var result = await controller.Post(fakeMessages);
            var okResult = result as BadRequestObjectResult;
            //Assert
            Assert.IsInstanceOf(typeof(BadRequestObjectResult), result);
            Assert.AreEqual("Invalid message to send", okResult.Value);
            Assert.AreEqual(400, okResult.StatusCode);
        }

        [Test]
        public async Task PostReturnsLogMessageForValidMessages()
        {
            //Arrange
            var fakeMessages = new LogMessageModel()
            {
                Messages = new List<Message>()
                {
                     new Message
                     {
                          Title = "Test message summary",
                          Text = "Exceptiion occured at ..."
                     }
                }.ToArray()
            };

            var fakeLogMessageResponse = new LogMessageResponse
            {
                Records = new List<RecordResponse>()
                {
                     new RecordResponse
                     {
                          Id = Guid.NewGuid().ToString(),
                           Field = new Field()
                           {
                                Id = "recTPWgTQaKlETabD",
                                Message= "Test message summary",
                                Summary = "Exceptiion occured at ...",
                                ReceivedAt = DateTime.UtcNow
                           },

                          CreatedTime = DateTime.UtcNow
                     }
                }.ToArray()
            };

            var mockLogMessageManager = new Mock<ILogMessageManager>();
            mockLogMessageManager.Setup(x => x.SendLogMessageAsync(It.IsAny<Message[]>())).Returns(Task.FromResult(fakeLogMessageResponse));

            var mockLogger = new Mock<ILogger<MessageController>>();
            var controller = new MessageController(mockLogMessageManager.Object, mockLogger.Object);

            //Act
            var result = await controller.Post(fakeMessages);
            var okResult = result as OkObjectResult;
            //Assert
            Assert.IsInstanceOf(typeof(LogMessageResponse), okResult.Value);
            Assert.IsNotNull(okResult.Value);
            var response = okResult.Value as LogMessageResponse;
            Assert.AreEqual(fakeLogMessageResponse.Records.Length, response.Records.Length);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [Test]
        public async Task GetReturnsLogMessages()
        {
            //Arrange
            var fakeLogMessageResponse = new LogMessageResponse
            {
                Records = new List<RecordResponse>()
                {
                     new RecordResponse
                     {
                          Id = Guid.NewGuid().ToString(),
                           Field = new Field()
                           {
                                Id = "recTPWgTQaKlETabD",
                                Message= "Test message summary",
                                Summary = "Exceptiion occured at ...",
                                ReceivedAt = DateTime.UtcNow
                           },

                          CreatedTime = DateTime.UtcNow
                     }
                }.ToArray()
            };

            var mockLogMessageManager = new Mock<ILogMessageManager>();
            mockLogMessageManager.Setup(x => x.GetLogMessagesAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(fakeLogMessageResponse));

            var mockLogger = new Mock<ILogger<MessageController>>();
            var controller = new MessageController(mockLogMessageManager.Object, mockLogger.Object);

            //Act
            var result = await controller.Get("1", "Grid+view");
            var okResult = result as OkObjectResult;
            //Assert
            Assert.IsInstanceOf(typeof(LogMessageResponse), okResult.Value);
            Assert.IsNotNull(okResult.Value);
            var response = okResult.Value as LogMessageResponse;
            Assert.AreEqual(fakeLogMessageResponse.Records.Length, response.Records.Length);
            Assert.AreEqual(200, okResult.StatusCode);
        }
    }
}
