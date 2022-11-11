using CookingBook.Domain.Exceptions;

namespace CookingBook.Domain.ValueObjects;

public class Ingredient
{
    public string Name { get; set; }
    public ushort Grams { get; set; }
    public ushort CaloriesPerHundredGrams { get; set; }
    
    public RecipeId RecipeId { get; set; }
    
    public Ingredient(string name, ushort grams, ushort caloriesPerHundredGrams)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new EmptyIngredientNameException();
        }
        Name = name;
        Grams = grams;
        CaloriesPerHundredGrams = caloriesPerHundredGrams;
    }

    public double getCalories()
        =>1.0*Grams/100.0 * CaloriesPerHundredGrams;
        
    
        
}