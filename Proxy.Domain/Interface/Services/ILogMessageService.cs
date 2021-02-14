using Proxy.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Proxy.Domain.Interface.Services
{
    public interface ILogMessageService
    {
        Task<string> SendLogsAsync(LogMessageRequest message);
        Task<string> GetLogsAsync(string maxRecords, string view);
    }
}
