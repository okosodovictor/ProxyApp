using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Proxy.Domain.Exceptions;
using Proxy.Domain.Interface.Managers;
using Proxy.Domain.Models;
using Proxy.Web.Interface.Services;
using Proxy.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proxy.Web.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IAccountManager _accountManager;
        private readonly ITokenService _tokenService;
        private readonly ILogger<AccountController> _logger;
        public AccountController(IAccountManager accountManager,
            ITokenService tokenService,
            ILogger<AccountController> logger)
        {
            _logger = logger;
            _accountManager = accountManager;
            _tokenService = tokenService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public ActionResult<TokenModel> Authenticate([FromBody] User user)
        {
            try
            {
                var validuser = _accountManager.ValidateUser(user.Username, user.Password);
                var token = _tokenService.GenerateToken(validuser);
                return token;
            }
            catch (ForbiddenException fbex)
            {
                _logger.LogError("Operation is Forbidden: {message}", fbex.Message);
                return Forbid(fbex.Message);
            }
            catch (ApplicationException ex)
            {
                _logger.LogError("Could not Authenticate: {message}", ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
