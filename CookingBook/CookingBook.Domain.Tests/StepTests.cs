using CookingBook.Domain.Exceptions;
using CookingBook.Domain.ValueObjects;
using Shouldly;

namespace CookingBook.Domain.Tests;

public class StepTests
{
    [Fact]
    public void
        StepConstructor_Throws_EmptyStepNameException_When_Name_Is_Empty()
    {
        var exception = Record.Exception(() => new Step(""));

        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<EmptyStepNameException>();
    }
    
       
    [Fact]
    public void
        StepConstructor_Creates_New_Step_When_Proper_Parameters_Are_Given()
    {
        Step step = new Step("Step 321");
        
        var exception = Record.Exception(() => step = new Step("Step"));

        exception.ShouldBeNull();
        step.Name.ShouldBe("Step");
    }
}