using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Proxy.Domain.Interface.Services;
using Proxy.Domain.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Proxy.Infrastructure.Services
{
    public class LogMessageService : ILogMessageService
    {
        private IHttpClientFactory _clientFactory;
        private readonly ILogger<LogMessageService> _logger;

        public LogMessageService(IHttpClientFactory factory, ILogger<LogMessageService> logger)
        {
            _clientFactory = factory;
             _logger = logger;
        }

        public async Task<string> SendLogsAsync(LogMessageRequest message)
        {
            _logger.LogInformation("At SendLogMessageAsync");
            var request = new HttpRequestMessage(HttpMethod.Post, "messages");
            string messageJson = JsonConvert.SerializeObject(message);
            request.Content = new StringContent(messageJson, Encoding.UTF8, "application/json");
            var client = _clientFactory.CreateClient("externalservice");
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new Exception((int)response.StatusCode + "-" + response.StatusCode.ToString());
            }
        }

        public async Task<string> GetLogsAsync(string maxRecords, string view)
        {
            _logger.LogInformation("At SendLogMessageAsync");
            var request = new HttpRequestMessage(HttpMethod.Get, $"messages?maxRecords={maxRecords}&view={HttpUtility.UrlEncode(view)}");
            var client = _clientFactory.CreateClient("externalservice");
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new Exception((int)response.StatusCode + "-" + response.StatusCode.ToString());
            }
        }
    }
}
