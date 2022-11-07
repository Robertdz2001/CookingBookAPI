using CookingBook.Domain.Exceptions;
using CookingBook.Domain.ValueObjects;

namespace CookingBook.Domain.Entities;

public class Recipe
{
    public RecipeId Id { get; private set; }
    private RecipeName _name;
    private RecipeImageUrl _imageUrl;
    private RecipePrepTime _prepTime; //in mins
    private RecipeCalories _calories;
    private RecipeCreatedDate _createdDate;
    private LinkedList<Tool> _tools =new();
    private LinkedList<Step> _steps =new();
    private LinkedList<Ingredient> _ingredients=new();
    
    public Recipe(RecipeName name, RecipeImageUrl imageUrl, RecipePrepTime prepTime, RecipeCreatedDate createdDate, RecipeId id)
    {
        _name = name;
        _imageUrl = imageUrl;
        _prepTime = prepTime;
        _createdDate = createdDate;
        Id = id;
        _calories = 0;
    }

    public void AddIngredient(Ingredient ingredient)
    {
        var alreadyExists = _ingredients.Any(i => i.Name.Equals(ingredient.Name));

        if (alreadyExists)
        {
            throw new IngredientAlreadyExistsExeption(ingredient.Name);
        }

        _calories += 1.0*ingredient.Grams/100.0 * ingredient.CaloriesPerHundredGrams;
        
        _ingredients.AddLast(ingredient);
    }

    public void ChangeIngredient(Ingredient ingredient, string ingredientName)
    {
        var ingredientToChange = getIngredient(ingredientName);
        
        _calories -=1.0*ingredientToChange.Grams/100.0 * ingredientToChange.CaloriesPerHundredGrams;
        
        _ingredients.Find(ingredientToChange).Value = ingredient;
        
        _calories += 1.0*ingredient.Grams/100.0 * ingredient.CaloriesPerHundredGrams;
    }
    
    private Ingredient getIngredient(string ingredientName)
    {
        var ingredient = _ingredients.FirstOrDefault(i => i.Name.Equals(ingredientName));

        if (ingredient is null)
        {
            throw new IngredientNotFoundException(ingredientName);
        }
        return ingredient;
    }

}