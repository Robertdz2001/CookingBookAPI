namespace CookingBook.Application.DTO;

public class RecipeDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string ImageUrl{ get; set; }
    public ushort PrepTime{ get; set; }
    public double Calories{ get; set; }
    public DateTime CreatedDate{ get; set; }
    
    public short RecipeRating { get; set; }
    public IEnumerable<ToolDto> Tools{ get; set; }
    public IEnumerable<StepDto> Steps { get; set; }
    public IEnumerable<IngredientDto> Ingredients{ get; set; }
    public IEnumerable<ReviewDto> Reviews{ get; set; }
}