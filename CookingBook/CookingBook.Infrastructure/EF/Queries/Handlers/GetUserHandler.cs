using Microsoft.EntityFrameworkCore;
using CookingBook.Application.DTO;
using CookingBook.Application.Queries;
using CookingBook.Application.Services;
using CookingBook.Infrastructure.EF.Contexts;
using CookingBook.Shared.Abstractions.Queries;

namespace CookingBook.Infrastructure.EF.Queries.Handlers;

public class GetUserHandler : IQueryHandler<GetUser,UserDto>
{
    private readonly ReadDbContext _context;
    private readonly IUserContextService _userContext;
    
    public GetUserHandler(ReadDbContext context, IUserContextService userContext)
    {
        _context = context;
        _userContext = userContext;
    }

    public async Task<UserDto> HandleAsync(GetUser query)
    {
      var user =  await _context.Users.FirstOrDefaultAsync(u => u.Id == _userContext.GetUserId);


      return user.AsDto();
    }
    

}