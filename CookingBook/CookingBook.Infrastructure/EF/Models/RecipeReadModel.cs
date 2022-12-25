﻿namespace CookingBook.Infrastructure.EF.Models;

public class RecipeReadModel
{
    public Guid Id { get; set; }
    public int Version { get; set; }
    public string Name { get; set; }
    public string ImageUrl{ get; set; }
    public ushort PrepTime{ get; set; }
    public double Calories{ get; set; }
    public DateTime CreatedDate{ get; set; }

    public short RecipeRating { get; set; } = 0;
    public IEnumerable<ToolReadModel> Tools{ get; set; }
    public IEnumerable<StepReadModel> Steps { get; set; }
    public IEnumerable<IngredientReadModel> Ingredients{ get; set; }
    
    public IEnumerable<ReviewReadModel> Reviews{ get; set; }
    public Guid UserId { get; set; }
    public UserReadModel User { get; set; }
}