using CookingBook.Domain.Entities;

namespace CookingBook.Infrastructure.EF.Models;

public class UserReadModel
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string PasswordHash { get; set; }

    public short UserRating { get; set; } = 0;
    
    public Role Role { get; set; }

    public int RoleId { get; set; } = 1;
    public IEnumerable<RecipeReadModel> Recipes { get; set; }
}