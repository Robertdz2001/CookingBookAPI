namespace CookingBook.Infrastructure.EF.Models;

public class ToolReadModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public ushort Quantity { get; set; }
    
    public RecipeReadModel? Recipe { get; set; }
    public Guid? RecipeId { get; set; }
}