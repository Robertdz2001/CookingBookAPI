using CookingBook.Domain.Events;
using CookingBook.Domain.Exceptions;
using CookingBook.Domain.ValueObjects;
using CookingBook.Shared.Abstractions.Domain;

namespace CookingBook.Domain.Entities;

public class Recipe : AggregateRoot<RecipeId>
{
    //public RecipeId Id from AggregateRoot
    private RecipeName _name;
    private RecipeImageUrl _imageUrl;
    private RecipePrepTime _prepTime; //in mins
    private RecipeCalories _calories;
    private RecipeCreatedDate _createdDate;
    private LinkedList<Tool> _tools =new();
    private LinkedList<Step> _steps =new();
    private LinkedList<Ingredient> _ingredients=new();
    public UserId UserId { get; set; }
    
    
    public Recipe(UserId userId ,RecipeId id,RecipeName name, RecipeImageUrl imageUrl, RecipePrepTime prepTime, RecipeCreatedDate createdDate)
    {
        UserId = userId;
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
            throw new IngredientAlreadyExistsException(ingredient.Name);
        }

        _calories += 1.0*ingredient.Grams/100.0 * ingredient.CaloriesPerHundredGrams;
        
        _ingredients.AddLast(ingredient);
        
        AddEvent(new IngredientAdded(this, ingredient));
    }

    public void ChangeIngredient(Ingredient ingredient, string ingredientName)
    {
        var ingredientToChange = getIngredient(ingredientName);
        
        var alreadyExists = _ingredients.Any(i => i.Name.Equals(ingredient.Name));
        
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
        var ingredientToRemove = getIngredient(ingredientName);
        
        _calories -= ingredientToRemove.getCalories();

        _ingredients.Remove(ingredientToRemove);
        
        AddEvent(new IngredientRemoved(this, ingredientToRemove));
    }
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
        var toolToChange = getTool(toolName);
        
        var alreadyExists = _tools.Any(t => t.Name.Equals(tool.Name));

        if (alreadyExists)
        {
            throw new ToolAlreadyExistsException(tool.Name);
        }

        _tools.Find(toolToChange).Value = tool;
        
        AddEvent(new ToolChanged(this, toolToChange));
    }

    public void RemoveTool(string toolName)
    {
        var toolToRemove = getTool(toolName);

        _tools.Remove(toolToRemove);
        
        AddEvent(new ToolRemoved(this, toolToRemove));
    }
    
    
    
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
        var stepToChange = getStep(stepName);
        
        var alreadyExists = _steps.Any(s => s.Name.Equals(step.Name));

        if (alreadyExists)
        {
            throw new StepAlreadyExistsException(step.Name);
        }

        _steps.Find(stepToChange).Value = step;
        
        AddEvent(new StepChanged(this, stepToChange));
    }

    public void RemoveStep(string stepName)
    {
        var stepToRemove = getStep(stepName);

        _steps.Remove(stepToRemove);
        
        AddEvent(new StepRemoved(this, stepToRemove));
    }
    
    
    
    private Ingredient getIngredient(string ingredientName)
    {
        var ingredient = _ingredients.FirstOrDefault(t => t.Name.Equals(ingredientName));

        if (ingredient is null)
        {
            throw new IngredientNotFoundException(ingredientName);
        }
        return ingredient;
    }
    
    private Tool getTool(string toolName)
    {
        var tool = _tools.FirstOrDefault(t => t.Name.Equals(toolName));

        if (tool is null)
        {
            throw new ToolNotFoundException(toolName);
        }
        return tool;
    }
    private Step getStep(string stepName)
    {
        var step = _steps.FirstOrDefault(s => s.Name.Equals(stepName));

        if (step is null)
        {
            throw new StepNotFoundException(stepName);
        }
        return step;
    }

}