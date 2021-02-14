using Newtonsoft.Json;
using Proxy.Domain.Interface.Managers;
using Proxy.Domain.Interface.Services;
using Proxy.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxy.Domain.Managers
{
    public class LogMessageManager : ILogMessageManager
    {
        private readonly ILogMessageService _service;
        public LogMessageManager(ILogMessageService service)
        {
            _service = service;
        }

        public async Task<LogMessageResponse> GetLogMessagesAsync(string maxRecords, string view)
        {
            var response = await _service.GetLogsAsync(maxRecords, view);
            if (response != null)
            {
                var result = JsonConvert.DeserializeObject<LogMessageResponse>(response);
                return result;
            }

            return null;
        }

        public async Task<LogMessageResponse> SendLogMessageAsync(Message[] messages)
        {
            if (messages == null || !messages.Any())
            {
                throw new InvalidOperationException("Invalid message to send");
            }

            //fields
            var fields = messages.Select(x => new Field
            {
                Id = Guid.NewGuid().ToString(),
                Summary = x.Title,
                Message = x.Text,
                ReceivedAt = DateTime.UtcNow
            }).ToList();

            //records
            var records = fields.Select(field => new RecordRequest()
            {
                Field = field
            }).ToArray();

            //log message
            var logRequest = new LogMessageRequest()
            {
                Records = records
            };

            logRequest.Validate();
            var response = await _service.SendLogsAsync(logRequest);
            if (response != null)
            {
                var result = JsonConvert.DeserializeObject<LogMessageResponse>(response);
                return result;
            }

            return null;
        }
    }
}
