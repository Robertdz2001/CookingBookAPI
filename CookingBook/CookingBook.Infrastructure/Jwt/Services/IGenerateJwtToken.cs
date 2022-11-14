using CookingBook.Infrastructure.Jwt.DTO;

namespace CookingBook.Infrastructure.Jwt.Services;

public interface IGenerateJwtToken
{
    Task<string> Generate(LoginDto dto);
}