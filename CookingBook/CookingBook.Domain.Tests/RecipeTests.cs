using CookingBook.Domain.Entities;
using CookingBook.Domain.Events;
using CookingBook.Domain.Exceptions;
using CookingBook.Domain.Factories;
using CookingBook.Domain.ValueObjects;
using Shouldly;

namespace CookingBook.Domain.Tests;

public class RecipeTests
{
    #region Reviews

    [Fact]
    public void AddReview_Adds_ReviewAdded_Domain_Event_On_Success()
    {
        var recipe = GetRecipe();

        var review = new Review("Review 1", "Content", 5, Guid.NewGuid());

        var exception = Record.Exception(() => recipe.AddReview(review));
        
        exception.ShouldBeNull();

        var @event = recipe.Events.FirstOrDefault() as ReviewAdded;
        
        @event.ShouldNotBeNull();
        @event.Review.Name.ShouldBe("Review 1");

    }
    [Fact]
    public void AddReview_Throws_ReviewAlreadyExistsException_When_There_Is_Already_Review_With_The_Same_Name()
    {
        var recipe = GetRecipe();
        
        recipe.AddReview(new Review("Review 1", "Content" , 5, Guid.NewGuid()));
        
        var review = new Review("Review 1", "Content", 5, Guid.NewGuid());

        var exception = Record.Exception(() => recipe.AddReview(review));
        
        exception.ShouldNotBeNull();

        exception.ShouldBeOfType<ReviewAlreadyExistsException>();
    }
    [Fact]
    public void AddReview_Throws_ReviewAlreadyExistsException_When_User_Already_Added_Review()
    {
        var recipe = GetRecipe();

        var uid = Guid.NewGuid();
        
        recipe.AddReview(new Review("Review 3", "Content" , 5, uid));
        
        var review = new Review("Review 1", "Content", 5, uid);

        var exception = Record.Exception(() => recipe.AddReview(review));
        
        exception.ShouldNotBeNull();

        exception.ShouldBeOfType<ReviewAlreadyExistsException>();
    }
    
    [Fact]
    public void ChangeReview_Adds_ReviewChanged_Domain_Event_On_Success()
    {
        var recipe = GetRecipe();
        
        recipe.AddReview(new Review("ReviewToChange", "Content" , 5, Guid.NewGuid()));
        
        var review = new Review("Review 1", "Content", 5, Guid.NewGuid());
        
        recipe.ClearEvents();

        var exception = Record.Exception(() => recipe.ChangeReview(review,"ReviewToChange"));
        
        exception.ShouldBeNull();

        var @event = recipe.Events.FirstOrDefault() as ReviewChanged;
        
        @event.ShouldNotBeNull();
        @event.Review.Name.ShouldBe("ReviewToChange");
        
    }
    
    [Fact]
    public void ChangeReview_Throws_ReviewNotFoundException_When_There_Is_No_Review_With_Given_Name()
    {
        var recipe = GetRecipe();

        var review = new Review("Review 1", "Content", 5, Guid.NewGuid());

        var exception = Record.Exception(() => recipe.ChangeReview(review,"ReviewToChange"));
        
        exception.ShouldNotBeNull();

        exception.ShouldBeOfType<ReviewNotFoundException>();

    }
    
    [Fact]
    public void ChangeReview_Throws_ReviewAlreadyExistsException_When_There_Is_Already_Review_With_The_Same_Name()
    {
        var recipe = GetRecipe();
        
        recipe.AddReview(new Review("Review 1","Content",5, Guid.NewGuid()));
        recipe.AddReview(new Review("Review 2","Content",5, Guid.NewGuid()));

        var review = new Review("Review 2", "Content123", 5, Guid.NewGuid());

        var exception = Record.Exception(() => recipe.ChangeReview(review,"Review 1"));
        
        exception.ShouldNotBeNull();

        exception.ShouldBeOfType<ReviewAlreadyExistsException>();

    }
    
    [Fact]
    public void RemoveReview_Adds_ReviewRemoved_Domain_Event_On_Success()
    {
        var recipe = GetRecipe();
        
        recipe.AddReview(new Review("ReviewToRemove", "Content" , 5, Guid.NewGuid()));

        recipe.ClearEvents();

        var exception = Record.Exception(() => recipe.RemoveReview("ReviewToRemove"));
        
        exception.ShouldBeNull();

        var @event = recipe.Events.FirstOrDefault() as ReviewRemoved;
        
        @event.ShouldNotBeNull();
        @event.Review.Name.ShouldBe("ReviewToRemove");
        
    }
    
    [Fact]
    public void RemoveReview_Throws_ReviewNotFoundException_When_There_Is_No_Review_With_Given_Name()
    {
        var recipe = GetRecipe();

        var exception = Record.Exception(() => recipe.RemoveReview("ReviewToRemove"));
        
        exception.ShouldNotBeNull();

        exception.ShouldBeOfType<ReviewNotFoundException>();
        
    }
    #endregion
    
    #region Ingredients
    [Fact]
    public void
        AddIngredient_Throws_IngredientAlreadyExistsException_When_There_Is_Already_Ingredient_With_The_Same_Name()
    {
        var recipe = GetRecipe();
        
        recipe.AddIngredient(new Ingredient("Ingredient 1", 10, 10 ));

        var exception = Record.Exception(() 
            => recipe.AddIngredient(new Ingredient("Ingredient 1", 321, 321)));

        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<IngredientAlreadyExistsException>();
        
    }

    [Fact]
    public void
        AddIngredient_Adds_IngredientAdded_Domain_Event_On_Success()
    {
        var recipe = GetRecipe();
        
        var exception = Record.Exception(() 
            => recipe.AddIngredient(new Ingredient("Ingredient 1", 321, 321)));
        
        exception.ShouldBeNull();
        recipe.Events.Count().ShouldBe(1);

        var @event = recipe.Events.FirstOrDefault() as IngredientAdded;

        @event.ShouldNotBeNull();
        @event.Ingredient.Name.ShouldBe("Ingredient 1");

    }

    [Fact]
    public void
        ChangeIngredient_Throws_IngredientNotFoundException_When_There_Is_No_Ingredient_With_Given_Name()
    {
        var recipe = GetRecipe();
        var exception = Record.Exception(() 
            => recipe.ChangeIngredient(new Ingredient("Ingredient 1", 321, 321),"Ingredient 2"));

        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<IngredientNotFoundException>();
    }

    [Fact]
    public void
        ChangeIngredient_Throws_IngredientAlreadyExistsException_When_There_Is_Already_Ingredient_With_The_Same_Name()
    {
        var recipe = GetRecipe();
        
        recipe.AddIngredient(new Ingredient("Ingredient 1", 10, 10 ));
        recipe.AddIngredient(new Ingredient("Ingredient 2", 10, 10 ));
        var exception = Record.Exception(() 
            => recipe.ChangeIngredient(new Ingredient("Ingredient 1", 321, 321),"Ingredient 2"));

        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<IngredientAlreadyExistsException>();
    }

    [Fact]
    public void
        ChangeIngredient_Adds_IngredientChanged_Domain_Event_On_Success()
    {
        var recipe = GetRecipe();
        
        recipe.AddIngredient(new Ingredient("Ingredient 1", 10, 10 ));
        recipe.ClearEvents();
        var exception = Record.Exception(() 
            => recipe.ChangeIngredient(new Ingredient("Ingredient 321", 321, 321),"Ingredient 1"));
        
        exception.ShouldBeNull();
        
        recipe.Events.Count().ShouldBe(1);

        var @event = recipe.Events.FirstOrDefault() as IngredientChanged;
        
        @event.Ingredient.Name.ShouldBe("Ingredient 1");

    }

    [Fact]
    public void
        RemoveIngredient_Throws_IngredientNotFoundException_When_There_Is_No_Ingredient_With_Given_Name()
    {
        var recipe = GetRecipe();
        
        var exception = Record.Exception(() 
            => recipe.RemoveIngredient("Ingredient 1"));

        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<IngredientNotFoundException>();
    }
    
    
    [Fact]
    public void
        RemoveIngredient_Adds_IngredientRemoved_Domain_Event_On_Success()
    {
        var recipe = GetRecipe();
        recipe.AddIngredient(new Ingredient("Ingredient 1", 321, 321));
        recipe.ClearEvents();
        var exception = Record.Exception(() 
            => recipe.RemoveIngredient("Ingredient 1"));
        
        exception.ShouldBeNull();
        recipe.Events.Count().ShouldBe(1);

        var @event = recipe.Events.FirstOrDefault() as IngredientRemoved;

        @event.ShouldNotBeNull();
        @event.Ingredient.Name.ShouldBe("Ingredient 1");

    }
    
    #endregion

    #region Tools

    [Fact]
    public void
        AddTool_Throws_ToolAlreadyExistsException_When_There_Is_Already_Tool_With_The_Same_Name()
    {
        var recipe = GetRecipe();
        
        recipe.AddTool(new Tool("Tool 1", 10));

        var exception = Record.Exception(() 
            => recipe.AddTool(new Tool("Tool 1", 321)));

        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ToolAlreadyExistsException>();
        
    }

    [Fact]
    public void
        AddTool_Adds_ToolAdded_Domain_Event_On_Success()
    {
        var recipe = GetRecipe();
        
        var exception = Record.Exception(() 
            => recipe.AddTool(new Tool("Tool 1", 321)));
        
        exception.ShouldBeNull();
        recipe.Events.Count().ShouldBe(1);
    
        var @event = recipe.Events.FirstOrDefault() as ToolAdded;
    
        @event.ShouldNotBeNull();
        @event.Tool.Name.ShouldBe("Tool 1");
    
    }
    
    [Fact]
    public void
        ChangeTool_Throws_ToolNotFoundException_When_There_Is_No_Tool_With_Given_Name()
    {
        var recipe = GetRecipe();
        var exception = Record.Exception(() 
            => recipe.ChangeTool(new Tool("Tool 1", 321),"Tool 2"));
    
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ToolNotFoundException>();
    }
    
    [Fact]
    public void
        ChangeTool_Throws_ToolAlreadyExistsException_When_There_Is_Already_Tool_With_The_Same_Name()
    {
        var recipe = GetRecipe();
        
        recipe.AddTool(new Tool("Tool 1", 10));
        recipe.AddTool(new Tool("Tool 2", 10 ));
        var exception = Record.Exception(() 
            => recipe.ChangeTool(new Tool("Tool 1", 321),"Tool 2"));
    
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ToolAlreadyExistsException>();
    }
    
    [Fact]
    public void
        ChangeTool_Adds_ToolChanged_Domain_Event_On_Success()
    {
        var recipe = GetRecipe();
        
        recipe.AddTool(new Tool("Tool 1", 10));
        recipe.ClearEvents();
        var exception = Record.Exception(() 
            => recipe.ChangeTool(new Tool("Tool 321", 321),"Tool 1"));
        
        exception.ShouldBeNull();
        
        recipe.Events.Count().ShouldBe(1);
    
        var @event = recipe.Events.FirstOrDefault() as ToolChanged;
        
        @event.Tool.Name.ShouldBe("Tool 1");
    
    }
    
    [Fact]
    public void
        RemoveTool_Throws_ToolNotFoundException_When_There_Is_No_Tool_With_Given_Name()
    {
        var recipe = GetRecipe();
        
        var exception = Record.Exception(() 
            => recipe.RemoveTool("Tool 1"));
    
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ToolNotFoundException>();
    }
    
    
    [Fact]
    public void
        RemoveTool_Adds_ToolRemoved_Domain_Event_On_Success()
    {
        var recipe = GetRecipe();
        recipe.AddTool(new Tool("Tool 1", 321));
        recipe.ClearEvents();
        var exception = Record.Exception(() 
            => recipe.RemoveTool("Tool 1"));
        
        exception.ShouldBeNull();
        recipe.Events.Count().ShouldBe(1);
    
        var @event = recipe.Events.FirstOrDefault() as ToolRemoved;
    
        @event.ShouldNotBeNull();
        @event.Tool.Name.ShouldBe("Tool 1");
    
    }

    #endregion

    #region Steps
[Fact]
    public void
        AddStep_Throws_StepAlreadyExistsException_When_There_Is_Already_Step_With_The_Same_Name()
    {
        var recipe = GetRecipe();
        
        recipe.AddStep(new Step("Step 1"));

        var exception = Record.Exception(() 
            => recipe.AddStep(new Step("Step 1")));

        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<StepAlreadyExistsException>();
        
    }

    [Fact]
    public void
        AddStep_Adds_StepAdded_Domain_Event_On_Success()
    {
        var recipe = GetRecipe();
        
        var exception = Record.Exception(() 
            => recipe.AddStep(new Step("Step 1")));
        
        exception.ShouldBeNull();
        recipe.Events.Count().ShouldBe(1);
    
        var @event = recipe.Events.FirstOrDefault() as StepAdded;
    
        @event.ShouldNotBeNull();
        @event.Step.Name.ShouldBe("Step 1");
    
    }
    
    [Fact]
    public void
        ChangeStep_Throws_StepNotFoundException_When_There_Is_No_Step_With_Given_Name()
    {
        var recipe = GetRecipe();
        var exception = Record.Exception(() 
            => recipe.ChangeStep(new Step("Step 1"),"Step 2"));
    
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<StepNotFoundException>();
    }
    
    [Fact]
    public void
        ChangeStep_Throws_StepAlreadyExistsException_When_There_Is_Already_Step_With_The_Same_Name()
    {
        var recipe = GetRecipe();
        
        recipe.AddStep(new Step("Step 1"));
        recipe.AddStep(new Step("Step 2"));
        var exception = Record.Exception(() 
            => recipe.ChangeStep(new Step("Step 1"),"Step 2"));
    
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<StepAlreadyExistsException>();
    }
    
    [Fact]
    public void
        ChangeStep_Adds_StepChanged_Domain_Event_On_Success()
    {
        var recipe = GetRecipe();
        
        recipe.AddStep(new Step("Step 1"));
        recipe.ClearEvents();
        var exception = Record.Exception(() 
            => recipe.ChangeStep(new Step("Step 321"),"Step 1"));
        
        exception.ShouldBeNull();
        
        recipe.Events.Count().ShouldBe(1);
    
        var @event = recipe.Events.FirstOrDefault() as StepChanged;
        
        @event.Step.Name.ShouldBe("Step 1");
    
    }
    
    [Fact]
    public void
        RemoveStep_Throws_StepNotFoundException_When_There_Is_No_Step_With_Given_Name()
    {
        var recipe = GetRecipe();
        
        var exception = Record.Exception(() 
            => recipe.RemoveStep("Step 1"));
    
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<StepNotFoundException>();
    }
    
    
    [Fact]
    public void
        RemoveStep_Adds_ToolRemoved_Domain_Event_On_Success()
    {
        var recipe = GetRecipe();
        recipe.AddStep(new Step("Step 1"));
        recipe.ClearEvents();
        var exception = Record.Exception(() 
            => recipe.RemoveStep("Step 1"));
        
        exception.ShouldBeNull();
        recipe.Events.Count().ShouldBe(1);
    
        var @event = recipe.Events.FirstOrDefault() as StepRemoved;
    
        @event.ShouldNotBeNull();
        @event.Step.Name.ShouldBe("Step 1");
    
    }
    

    #endregion
    
    #region ARRANGE
        
    private Recipe GetRecipe()
    {
        var recipe = _factory.Create(Guid.NewGuid(),Guid.NewGuid(),"Name","http://url.com",30);
        recipe.ClearEvents();
        return recipe;
    }

    private readonly IRecipeFactory _factory;

    public RecipeTests()
    {
        _factory = new RecipeFactory();
    }

    #endregion
}