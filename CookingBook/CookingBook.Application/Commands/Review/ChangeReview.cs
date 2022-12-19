using CookingBook.Shared.Abstractions.Commands;

namespace CookingBook.Application.Commands.Review;

public record ChangeReview(Guid RecipeId, string Name, string Content, short Rate, string ReviewToChange) : ICommand;
