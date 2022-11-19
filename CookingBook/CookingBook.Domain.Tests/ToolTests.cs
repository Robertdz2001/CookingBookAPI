using CookingBook.Domain.Exceptions;
using CookingBook.Domain.ValueObjects;
using Shouldly;

namespace CookingBook.Domain.Tests;

public class ToolTests
{
    [Fact]
    public void
        ToolConstructor_Throws_EmptyToolNameException_When_Name_Is_Empty()
    {
        var exception = Record.Exception(() => new Tool("", 321));

        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<EmptyToolNameException>();
    }
    
       
    [Fact]
    public void
        ToolConstructor_Creates_New_Tool_When_Proper_Parameters_Are_Given()
    {
        Tool tool = new Tool("Tool 321", 321);
        
        var exception = Record.Exception(() => tool = new Tool("Tool", 321));

        exception.ShouldBeNull();
        tool.Name.ShouldBe("Tool");
    }
}