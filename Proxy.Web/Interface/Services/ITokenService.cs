using Proxy.Domain.Models;
using Proxy.Web.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace Proxy.Web.Interface.Services
{
    public interface ITokenService
    {
        TokenModel GenerateToken(User user);
        bool ValidateCurrentToken(string token, out JwtSecurityToken jwtValidatedToken);
    }
}
