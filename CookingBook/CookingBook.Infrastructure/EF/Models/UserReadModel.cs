using CookingBook.Domain.Consts;

namespace CookingBook.Infrastructure.EF.Models;

public class UserReadModel
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string PasswordHash { get; set; }
    public Role UserRole { get; set; }
    
    public IEnumerable<RecipeReadModel> Recipes { get; set; }
}