namespace CookingBook.Infrastructure.EF.Models;

public class ReviewReadModel
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public DateTime CreatedDate { get; set; }
    
    public string Content { get; set; }
    
    public short Rate { get; set; }
    
    public UserReadModel User {get; set;}
    
    public Guid UserId { get; set; }
    
    public RecipeReadModel Recipe { get; set; }
    
    public Guid RecipeId { get; set; }
}