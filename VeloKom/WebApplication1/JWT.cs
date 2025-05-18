using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace VeloKom
{
    public static class JWT
    {
        public static string GenerateToken(Users user)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("secretKeyseretKeysecretKeysecretKeysecretKeysecretKeysecretKeysecretKeysecretKeysecretKey"));
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var claim = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var token = new JwtSecurityToken(
                claims: claim,
                expires: DateTime.Now.AddHours(12),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public static void SaveToken(HttpResponse response, string token)
        {
            var cookiesOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTime.Now.AddHours(12)
            };
            response.Cookies.Append("AuthToken", token, cookiesOptions);
        }
        public static string GetToken(HttpRequest request)
        {
            if(request.Cookies.TryGetValue("AuthToken", out var token))
            {
                return token;
            }
            return null;
        }
        public static string GetUserIdFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            if (handler.CanReadToken(token))
            {
                var jwtToken = handler.ReadJwtToken(token);
                var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
                return userId;
            }
            return null;
        }
        public static void DeleteToken(HttpResponse response)
        {
            response.Cookies.Delete("AuthToken");
        }
    }
}
