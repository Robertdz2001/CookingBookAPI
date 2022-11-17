using CookingBook.Domain.Exceptions;
using CookingBook.Domain.ValueObjects;
using Shouldly;

namespace CookingBook.Domain.Tests;

public class IngredientTests
{
    [Fact]
    public void
        IngredientConstructor_Throws_EmptyIngredientNameException_When_Name_Is_Empty()
    {
        var exception = Record.Exception(() => new Ingredient("", 321, 321));

        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<EmptyIngredientNameException>();
    }
    
       
    [Fact]
    public void
        IngredientConstructor_Creates_New_Ingredient_When_Proper_Parameters_Are_Given()
    {
        Ingredient ingredient = new Ingredient("Ingredient 321", 321, 321);
        
        var exception = Record.Exception(() => ingredient = new Ingredient("Ingredient", 321, 321));

        exception.ShouldBeNull();
        ingredient.Name.ShouldBe("Ingredient");
    }

    [Fact]
    public void
        GetCalories_Returns_Correct_Value()
    {
        Ingredient ingredient = new Ingredient("Ingredient", 75, 300);

        var result = ingredient.getCalories();
        
        
        result.ShouldBe(225);



    }
}