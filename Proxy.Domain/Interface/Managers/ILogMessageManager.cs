using Proxy.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Proxy.Domain.Interface.Managers
{
    public interface ILogMessageManager
    {
        Task<LogMessageResponse> SendLogMessageAsync(Message[] messages);
        Task<LogMessageResponse> GetLogMessagesAsync(string maxRecords, string view);
    }
}
