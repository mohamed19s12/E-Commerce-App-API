using Ecom.Core.Entities;
using Ecom.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Infrastructure.Repositories.Services
{
    public class GenerateTokenService : IGenerateTokenService
    {
        private readonly IConfiguration config;

        public GenerateTokenService(IConfiguration config)
        {
            this.config = config;
        }

        public string GetAndCreateToken(AppUser user)
        {
            //list of claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName)
            };


            //create token key
            var security = config["Token:Secret"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(security));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //create token descriptor
            SecurityTokenDescriptor tokendesc = new()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                Issuer = config["Token:Issuer"],
                SigningCredentials = credentials,
                NotBefore = DateTime.UtcNow,

            };

            //create token handler

            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(tokendesc);

            return handler.WriteToken(token);
        }
    }
}
