using Microsoft.AspNetCore.Http;
using Proxy.Web.Extensions;
using Proxy.Web.Interface.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Proxy.Web.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ITokenService tokenService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                SetUserOnContext(context, token, tokenService);

            await _next(context);
        }

        private static void SetUserOnContext(HttpContext context, string tokenString, ITokenService _tokenService)
        {
            try
            {
                if (_tokenService.ValidateCurrentToken(tokenString, out JwtSecurityToken jwtToken))
                {
                    var identity = new ClaimsIdentity(jwtToken.Claims, Constants.AuthScheme.JWT);
                    context.User = new ClaimsPrincipal(identity);
                }
            }
            catch
            {
                // do nothing if jwt validation fails
                // user is not attached to context so request won't have access to secure routes
            }
        }
    }
}
