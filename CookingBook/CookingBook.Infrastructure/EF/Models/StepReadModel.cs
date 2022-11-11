namespace CookingBook.Infrastructure.EF.Models;

public class StepReadModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    
    public RecipeReadModel? Recipe { get; set; }
    public Guid? RecipeId { get; set; }
}