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
    private LinkedList<Tool> _tools;
    private LinkedList<Step> _steps;
    private LinkedList<Ingredient> _ingredients;

}