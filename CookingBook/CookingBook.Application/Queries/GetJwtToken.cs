using CookingBook.Infrastructure.Jwt.DTO;
using CookingBook.Shared.Abstractions.Queries;

namespace CookingBook.Application.Queries;

public class GetJwtToken : IQuery<string>
{
    public LoginDto Dto { get; set; }
}