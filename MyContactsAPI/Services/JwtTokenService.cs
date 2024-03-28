using Microsoft.IdentityModel.Tokens;
using MyContactsAPI.Extensions;
using MyContactsAPI.Interfaces;
using MyContactsAPI.Models.UserModels;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MyContactsAPI.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _configuration;

        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Configuration.Secrets.JwtPrivateKey);            
            var credentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature);



            var tokenDescriptor = new SecurityTokenDescriptor
            {
                SigningCredentials = credentials,
                Expires = DateTime.UtcNow.AddSeconds(1),
                Subject = CreateIdentity(user)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private static ClaimsIdentity CreateIdentity(User user)
        {
            var claimIdentity = new ClaimsIdentity();

            claimIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claimIdentity.AddClaim(new Claim(ClaimTypes.GivenName, user.Name));
            claimIdentity.AddClaim(new Claim(ClaimTypes.Email, user.Email));


            return claimIdentity;
        }
    }
}