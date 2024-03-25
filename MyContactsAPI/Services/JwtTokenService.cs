using Microsoft.IdentityModel.Tokens;
using MyContactsAPI.Interfaces;
using MyContactsAPI.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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
            var key = Encoding.ASCII.GetBytes(_configuration["Secrets:JwtPrivateKey"]);

            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                SigningCredentials = credentials,
                Expires = DateTime.UtcNow.AddHours(2), // Define o tempo de expiração do token (2 horas neste exemplo)
                Subject = CreateIdentity(user)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private static ClaimsIdentity CreateIdentity(User user)
        {
            var claimIdentity = new ClaimsIdentity();

            claimIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())); // Inclua apenas o ID do usuário como reivindicação

            return claimIdentity;
        }

        public Guid ValidateJwtToken(string authorizationHeader)
        {
            if (string.IsNullOrEmpty(authorizationHeader))
            {
                throw new ArgumentException("Authorization header is null or empty.");
            }

            var token = ExtractToken(authorizationHeader);
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentException("Token is null or empty.");
            }

            var tokenHandler = new JwtSecurityTokenHandler();

            var validationParameters = CreateValidationParameters();

            try
            {
                var claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

                if (IsTokenExpired(validatedToken))
                {
                    throw new SecurityTokenExpiredException("Token has expired.");
                }

                return ExtractUserIdFromClaims(claimsPrincipal);
            }
            catch (Exception ex)
            {
                throw new SecurityTokenValidationException("Error validating JWT token.", ex);
            }
        }

        private string ExtractToken(string authorizationHeader)
        {
            return authorizationHeader.Substring("Bearer ".Length).Trim();
        }

        private TokenValidationParameters CreateValidationParameters()
        {
            return new TokenValidationParameters
            {
                RequireExpirationTime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Secrets:JwtPrivateKey"])),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        }

        private bool IsTokenExpired(SecurityToken validatedToken)
        {
            return validatedToken.ValidTo < DateTime.UtcNow;
        }

        private Guid ExtractUserIdFromClaims(ClaimsPrincipal claimsPrincipal)
        {
            return Guid.Parse(claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }

        private void HandleValidationException(Exception ex)
        {
            Console.WriteLine($"Error validating JWT token: {ex.Message}");
        }
    }
}