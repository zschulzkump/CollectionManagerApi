using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CollectionManagerApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace CollectionManagerApi.Services;

public class TokenProviderService : ITokenProviderService
{
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;
    public TokenProviderService(
        UserManager<User> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }
    
    public async Task<string> CreateAccessTokenAsync(User user)
    {
        var roles = await _userManager.GetRolesAsync(user);

        var signingKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!));
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        List<Claim> claims =
        [
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            ..roles.Select(r => new Claim(ClaimTypes.Role, r))
        ];

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("Jwt:ExpirationInMinutes")),
            SigningCredentials = credentials,
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
        };
        
        var tokenHandler = new JsonWebTokenHandler();
        var accessToken = tokenHandler.CreateToken(tokenDescriptor);
        return accessToken;
    }

    public string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
    }
}