using CookingBook.Domain.Exceptions;
using CookingBook.Domain.ValueObjects;
using Shouldly;

namespace CookingBook.Domain.Tests;

public class ReviewTests
{
    [Fact]
    public void ReviewConstructor_Throws_EmptyReviewNameException_When_Name_Is_Empty()
    {
        var exception = Record.Exception(() => new Review("", "Content", 5));

        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<EmptyReviewNameException>();
    }
    
    [Fact]
    public void ReviewConstructor_Throws_EmptyReviewContentException_When_Name_Is_Empty()
    {
        var exception = Record.Exception(() => new Review("Review", "", 5));

        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<EmptyReviewContentException>();
    }
    
    [Theory]
    [InlineData(-6)]
    [InlineData(-12345)]
    [InlineData(6)]
    [InlineData(100)]
    public void ReviewConstructor_Throws_InvalidReviewRateException_When_Rate_Is_Not_Between_Minus_5_And_5(short rate)
    {
        var exception = Record.Exception(() => new Review("Review", "Content", rate));

        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<InvalidReviewRateException>();
    }
}