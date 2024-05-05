using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Repository;

public class AuthenticationManager : IAuthenticationManager
{
    
    private readonly UserManager<ApplicationUser> _userManager; 
    private readonly IConfiguration _configuration;
    private ApplicationUser _applicationUser;
    
    public AuthenticationManager(UserManager<ApplicationUser> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }
    
    public async Task<bool> ValidateUser(AuthenticationApplicationUserDto authenticationApplicationUserForAuth)
    {
        _applicationUser = await _userManager.FindByNameAsync(authenticationApplicationUserForAuth.UserName);
        return (_applicationUser != null && await _userManager.CheckPasswordAsync(_applicationUser,
            authenticationApplicationUserForAuth.Password));
    }
    public async Task<string> CreateToken()
    {
        var signingCredentials = GetSigningCredentials();
        var claims = await GetClaims();
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }
    
    private SigningCredentials GetSigningCredentials()
    {
        var secretKey = _configuration["JwtSettings:SecretKey"]; 
        if (string.IsNullOrEmpty(secretKey))
        {
            throw new ArgumentException("Secret key cannot be null or empty.", nameof(secretKey));
        }
        var key = Encoding.UTF8.GetBytes(secretKey);
        var secret = new SymmetricSecurityKey(key);
        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }
    // var secretKey = _configuration["JwtSettings:SecretKey"]; 
    // var secretKey = _configuration["CulinaryChestSecretKey"];
    // var key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable(secretKey));;
    // var secret = new SymmetricSecurityKey(key);
    // return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);

    private async Task<List<Claim>> GetClaims()
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, _applicationUser.UserName)
        };
        var roles = await _userManager.GetRolesAsync(_applicationUser); foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role)); 
        }
        return claims;
    }
    
    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims) {
        var jwtSettings = _configuration.GetSection("JwtSettings"); 
        var tokenOptions = new JwtSecurityToken
        (
            issuer: jwtSettings.GetSection("validIssuer").Value, 
            audience: jwtSettings.GetSection("validAudience").Value, 
            claims: claims,
            expires:
            DateTime.Now.AddYears((int)Convert.ToDouble(jwtSettings.GetSection("expires").Value)
            ), 
            signingCredentials: signingCredentials
        );
        return tokenOptions; 
    }
}