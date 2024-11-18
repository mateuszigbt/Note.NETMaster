using Microsoft.IdentityModel.Tokens;
using NoteApp.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Noteapp.Security
{
    /// <summary>
    /// Class responsible for generating and validating JWT tokens.
    /// </summary>
    public class JwtTokenProvider
    {
        private readonly string _secretKey;
        private readonly long _validityInMilliseconds;

        /// <summary>
        /// Initializes a new instance of the <see cref="JwtTokenProvider"/> class.
        /// </summary>
        /// <param name="configuration">The configuration containing JWT settings.</param>
        public JwtTokenProvider(IConfiguration configuration)
        {
            _secretKey = configuration["Jwt:Key"];
            _validityInMilliseconds = long.Parse(configuration["Jwt:Expiration"]);
        }

        /// <summary>
        /// Generates a JWT token for the specified user and roles.
        /// </summary>
        /// <param name="user">The user for whom the token is generated.</param>
        /// <param name="roles">The roles associated with the user.</param>
        /// <returns>The generated JWT token.</returns>
        public string GenerateToken(User user, List<Claim> roles)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email)
            };

            claims.AddRange(roles);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMilliseconds(_validityInMilliseconds),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Extracts the username from the JWT token.
        /// </summary>
        /// <param name="token">The JWT token.</param>
        /// <returns>The username extracted from the token.</returns>
        public string ExtractUsername(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            return jwtToken.Subject;
        }

        /// <summary>
        /// Validates the JWT token.
        /// </summary>
        /// <param name="token">The JWT token to validate.</param>
        /// <returns>True if the token is valid, otherwise false.</returns>
        public bool ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = key
                }, out SecurityToken validatedToken);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}