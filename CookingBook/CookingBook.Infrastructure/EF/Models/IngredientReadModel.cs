namespace CookingBook.Infrastructure.EF.Models;

public class IngredientReadModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public ushort Grams { get; set; }
    public ushort CaloriesPerHundredGrams { get; set; }
    
    public RecipeReadModel? Recipe { get; set; }
    public Guid? RecipeId { get; set; }
}