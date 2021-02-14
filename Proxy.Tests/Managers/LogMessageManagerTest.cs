using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using Proxy.Domain.Interface.Services;
using Proxy.Domain.Managers;
using Proxy.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Proxy.Tests.Managers
{
    public class LogMessageManagerTest
    {
        [Test]
        public async Task SendLogMessageAsync_Returns_InvalidOperationExceptionForEmptyMessages()
        {
            //Arrange
            var messages = new List<Message>()
            {
                //empty Message
            }.ToArray();

            var mockLogMessageManager = new Mock<ILogMessageService>();
            var manager = new LogMessageManager(mockLogMessageManager.Object);
            //Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await manager.SendLogMessageAsync(messages), "Invalid message to send");
        }

        [Test]
        public async Task SendLogMessageAsync_ReturnsResponse_ForValidMessages()
        {
            //Arrange
            var fakeMessages = new List<Message>()
            {
               new Message
                {
                    Title = "Test message summary",
                    Text = "Exceptiion occured at ..."
                }
            }.ToArray();

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

            var json = JsonConvert.SerializeObject(fakeLogMessageResponse);
            var mockLogMessageManager = new Mock<ILogMessageService>();

            mockLogMessageManager.Setup(x => x.SendLogsAsync(It.IsAny<LogMessageRequest>())).Returns(Task.FromResult(json));
            var manager = new LogMessageManager(mockLogMessageManager.Object);
            //Act
            var response = await manager.SendLogMessageAsync(fakeMessages);
            //Assert
            Assert.IsNotNull(response);
            mockLogMessageManager.Verify(s => s.SendLogsAsync(It.IsAny<LogMessageRequest>()), Times.Once);
        }

        [Test]
        public async Task GetLogMessagesAsync_ReturnsResponseMessages()
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
                     },
                     new RecordResponse
                     {
                          Id = Guid.NewGuid().ToString(),
                           Field = new Field()
                           {
                                Id = "rectH58h4TrecAdJG",
                                Message= "Test message summary",
                                Summary = "Exceptiion occured at ...",
                                ReceivedAt = DateTime.UtcNow
                           },

                          CreatedTime = DateTime.UtcNow
                     }
                }.ToArray()
            };

            var json = JsonConvert.SerializeObject(fakeLogMessageResponse);
            var mockLogMessageManager = new Mock<ILogMessageService>();

            mockLogMessageManager.Setup(x => x.GetLogsAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(json));
            var manager = new LogMessageManager(mockLogMessageManager.Object);
            //Act
            var response = await manager.GetLogMessagesAsync("2", "Grid+view");
            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(fakeLogMessageResponse.Records.Length, response.Records.Length);
            mockLogMessageManager.Verify(s => s.GetLogsAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}
