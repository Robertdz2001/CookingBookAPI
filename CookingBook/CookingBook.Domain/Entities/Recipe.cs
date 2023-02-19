using CookingBook.Domain.Events;
using CookingBook.Domain.Exceptions;
using CookingBook.Domain.ValueObjects;
using CookingBook.Shared.Abstractions.Domain;

namespace CookingBook.Domain.Entities;

public class Recipe : AggregateRoot<RecipeId>
{
    //public RecipeId Id from AggregateRoot
    private RecipeName _name;
    private ImageUrl _imageUrl;
    private RecipePrepTime _prepTime; //in mins
    private RecipeCalories _calories;
    private RecipeCreatedDate _createdDate;
    private RecipeRating _recipeRating;
    private LinkedList<Tool> _tools =new();
    private LinkedList<Step> _steps =new();
    private LinkedList<Ingredient> _ingredients=new();
    private LinkedList<Review> _reviews = new();
    public UserId UserId { get; set; }
    
    
    public Recipe(UserId userId ,RecipeId id,RecipeName name, ImageUrl imageUrl, RecipePrepTime prepTime, RecipeCreatedDate createdDate)
    {
        UserId = userId;
        _name = name;
        _imageUrl = imageUrl;
        _prepTime = prepTime;
        _createdDate = createdDate;
        Id = id;
        _calories = 0;
        _recipeRating = 0;
    }

    #region Review

    public void AddReview(Review review)
    {
        var userIsRecipeAuthor = review.UserId == UserId;

        if (userIsRecipeAuthor)
        {
            throw new UserIsRecipeAuthorException();
        }
        
        var alreadyExists = _reviews.Any(r => r.Name == review.Name);

        if (alreadyExists)
        {
            throw new ReviewAlreadyExistsException($"Review with name: '{review.Name}' already exists.");
        }

        

        var alreadyHasReviewFromUser = _reviews.Any(r => r.UserId == review.UserId);
        
        if(alreadyHasReviewFromUser)
        {
            throw new ReviewAlreadyExistsException($"User with id: '{review.UserId}' already added review for recipe with id: '{this.Id}'.");
        }

        _reviews.AddLast(review);

        _recipeRating += review.Rate;
        
        AddEvent(new ReviewAdded(this,review));
    }

    public void ChangeReview(Review review, string reviewName)
    {
        var reviewToChange = GetReview(reviewName);

        var alreadyExists = _reviews.Any(r => r.Name == review.Name && r.Name != reviewName);
        
        if (alreadyExists)
        {
            throw new ReviewAlreadyExistsException($"Review with name: '{review.Name}' already exists.");
        }

        _recipeRating -= reviewToChange.Rate;

        _reviews.Find(reviewToChange).Value = review;

        _recipeRating += review.Rate;
        
        AddEvent(new ReviewChanged(this, reviewToChange));
        
    }

    public void RemoveReview(string reviewName)
    {
        var reviewToRemove = GetReview(reviewName);

        _reviews.Remove(reviewToRemove);

        _recipeRating -= reviewToRemove.Rate;
        
        AddEvent(new ReviewRemoved(this,reviewToRemove));
        
    }
    
    public Review GetReview(string reviewName)
    {
        var review = _reviews.FirstOrDefault(r => r.Name == reviewName);

        if (review is null)
        {
            throw new ReviewNotFoundException(reviewName);
        }

        return review;
    }

    

    #endregion
    
    #region Ingredient
    public void AddIngredient(Ingredient ingredient)
    {
        var alreadyExists = _ingredients.Any(i => i.Name.Equals(ingredient.Name));
        
        if (alreadyExists)
        {
            throw new IngredientAlreadyExistsException(ingredient.Name);
        }

        _calories += 1.0*ingredient.Grams/100.0 * ingredient.CaloriesPerHundredGrams;
        
        _ingredients.AddLast(ingredient);
        
        AddEvent(new IngredientAdded(this, ingredient));
    }

    public void ChangeIngredient(Ingredient ingredient, string ingredientName)
    {
        var ingredientToChange = GetIngredient(ingredientName);
        
        var alreadyExists = _ingredients.Any(i => i.Name.Equals(ingredient.Name) && i.Name!=ingredientName);
        
        if (alreadyExists)
        {
            throw new IngredientAlreadyExistsException(ingredient.Name);
        }

        _calories -= ingredientToChange.getCalories();
        
        _ingredients.Find(ingredientToChange).Value = ingredient;
        
        _calories += ingredient.getCalories();
        
        AddEvent(new IngredientChanged(this, ingredientToChange));
    }

    public void RemoveIngredient(string ingredientName)
    {
        var ingredientToRemove = GetIngredient(ingredientName);
        
        _calories -= ingredientToRemove.getCalories();

        _ingredients.Remove(ingredientToRemove);
        
        AddEvent(new IngredientRemoved(this, ingredientToRemove));
    }
    
    private Ingredient GetIngredient(string ingredientName)
    {
        var ingredient = _ingredients.FirstOrDefault(t => t.Name.Equals(ingredientName));

        if (ingredient is null)
        {
            throw new IngredientNotFoundException(ingredientName);
        }
        return ingredient;
    }
    

    #endregion

    #region Tool
    public void AddTool(Tool tool)
    {
        var alreadyExists = _tools.Any(t => t.Name.Equals(tool.Name));

        if (alreadyExists)
        {
            throw new ToolAlreadyExistsException(tool.Name);
        }
        
        _tools.AddLast(tool);
        
        AddEvent(new ToolAdded(this, tool));
    }

    public void ChangeTool(Tool tool, string toolName)
    {
        var toolToChange = GetTool(toolName);
        
        var alreadyExists = _tools.Any(t => t.Name.Equals(tool.Name) && t.Name!=toolName);

        if (alreadyExists)
        {
            throw new ToolAlreadyExistsException(tool.Name);
        }

        _tools.Find(toolToChange).Value = tool;
        
        AddEvent(new ToolChanged(this, toolToChange));
    }

    public void RemoveTool(string toolName)
    {
        var toolToRemove = GetTool(toolName);

        _tools.Remove(toolToRemove);
        
        AddEvent(new ToolRemoved(this, toolToRemove));
    }
    private Tool GetTool(string toolName)
    {
        var tool = _tools.FirstOrDefault(t => t.Name.Equals(toolName));

        if (tool is null)
        {
            throw new ToolNotFoundException(toolName);
        }
        return tool;
    }
    
    #endregion

    #region Step

    public void AddStep(Step step)
    {
        var alreadyExists = _steps.Any(s => s.Name.Equals(step.Name));

        if (alreadyExists)
        {
            throw new StepAlreadyExistsException(step.Name);
        }
        
        _steps.AddLast(step);
        
        AddEvent(new StepAdded(this, step));
    }

    public void ChangeStep(Step step, string stepName)
    {
        var stepToChange = GetStep(stepName);
        
        var alreadyExists = _steps.Any(s => s.Name.Equals(step.Name)&& s.Name!=stepName);

        if (alreadyExists)
        {
            throw new StepAlreadyExistsException(step.Name);
        }

        _steps.Find(stepToChange).Value = step;
        
        AddEvent(new StepChanged(this, stepToChange));
    }

    public void RemoveStep(string stepName)
    {
        var stepToRemove = GetStep(stepName);

        _steps.Remove(stepToRemove);
        
        AddEvent(new StepRemoved(this, stepToRemove));
    }

    private Step GetStep(string stepName)
    {
        var step = _steps.FirstOrDefault(s => s.Name.Equals(stepName));

        if (step is null)
        {
            throw new StepNotFoundException(stepName);
        }
        return step;
    }

    #endregion

   

   
    

}