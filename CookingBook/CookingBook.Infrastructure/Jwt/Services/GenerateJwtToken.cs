using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CookingBook.Infrastructure.EF.Contexts;
using CookingBook.Infrastructure.EF.Models;
using CookingBook.Infrastructure.EF.Options;
using CookingBook.Infrastructure.Jwt.DTO;
using CookingBook.Shared.Abstractions.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CookingBook.Infrastructure.Jwt.Services;

public class GenerateJwtToken : IGenerateJwtToken
{
    private readonly DbSet<UserReadModel> _users;
    private readonly IPasswordHasher<UserReadModel> _passwordHasher;
    private readonly AuthenticationSettings _authenticationSettings;


    public GenerateJwtToken(ReadDbContext readDbContext, IPasswordHasher<UserReadModel> passwordHasher, AuthenticationSettings authenticationSettings)
    {
        _passwordHasher = passwordHasher;
        _authenticationSettings = authenticationSettings;
        _users = readDbContext.Users;
    }

    public async Task<string> Generate(LoginDto dto)
    {
        var user = await _users
            .FirstOrDefaultAsync(u => u.UserName.Equals(dto.UserName));

        if (user is null)
        {
            throw new CookingBookBadRequestException("Invalid userName or password");
        }
        
        var result = _passwordHasher.VerifyHashedPassword(user,user.PasswordHash,dto.Password);
        
        if(result == PasswordVerificationResult.Failed)
        {
            throw new CookingBookBadRequestException("Invalid username or password");
        }
        
        var claims =new List<Claim>()
        { 
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, $"{user.UserName}"),
            new Claim(ClaimTypes.Role, $"{user.UserRole}"),
        };
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

        var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer,
            _authenticationSettings.JwtIssuer,
            claims,
            expires:expires,
            signingCredentials: cred);


        var tokenHandler = new JwtSecurityTokenHandler();

        return tokenHandler.WriteToken(token);
    }
}