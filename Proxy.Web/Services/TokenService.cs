using IdentityModel;
using Microsoft.IdentityModel.Tokens;
using Proxy.Domain.Models;
using Proxy.Web.Interface.Services;
using Proxy.Web.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Proxy.Web.Services
{
    public class TokenService : ITokenService
    {
        private readonly Options _options;
        public TokenService(Options options)
        {
            _options = options;
        }

        public TokenModel GenerateToken(User user)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_options.Secret));
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtClaimTypes.Id, user.Id.ToString()),
                    new Claim(JwtClaimTypes.Name, user.Username),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                Issuer = _options.Issuer,
                Audience = _options.Audience,
                SigningCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtString = tokenHandler.WriteToken(token);
            return new TokenModel
            {
                DateCreated = DateTimeOffset.Now,
                Token = jwtString
            };
        }

        public bool ValidateCurrentToken(string token, out JwtSecurityToken jwtValidatedToken)
        {
            var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_options.Secret));

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _options.Issuer,
                    ValidAudience = _options.Audience,
                    IssuerSigningKey = mySecurityKey
                }, out SecurityToken validatedToken);

                jwtValidatedToken = (JwtSecurityToken)validatedToken;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Invalid JWT Token: {ex.Message}");
                jwtValidatedToken = null;
                return false;
            }
            return true;
        }

        public class Options
        {
            public string Secret { get; set; }
            public string Audience { get; set; }
            public string Issuer { get; set; }
        }
    }
}
