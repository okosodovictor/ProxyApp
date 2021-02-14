using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Proxy.Domain.Exceptions;
using Proxy.Domain.Interface.Managers;
using Proxy.Domain.Models;
using Proxy.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Proxy.Web.Controllers
{
    [Route("api/messages")]
    [ApiController]
    [Authorize]
    public class MessageController : Controller
    {
        private readonly ILogMessageManager _logMessageManager;
        private readonly ILogger<MessageController> _logger;
        public MessageController(ILogMessageManager logMessageManager,
            ILogger<MessageController> logger)
        {
            _logMessageManager = logMessageManager;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LogMessageModel model)
        {
            try
            {
                var response = await _logMessageManager.SendLogMessageAsync(model.Messages);
                return Ok(response);
            }
            catch (ApplicationException ex)
            {
                _logger.LogError($"Could not post Logs: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string maxRecords, [FromQuery] string view)
        {
            try
            {
                var result = await _logMessageManager.GetLogMessagesAsync(maxRecords, view);
                return Ok(result);
            }
            catch (ApplicationException ex)
            {
                _logger.LogError($"Could not Get Logs: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Could not Get Logs: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }
    }
}
