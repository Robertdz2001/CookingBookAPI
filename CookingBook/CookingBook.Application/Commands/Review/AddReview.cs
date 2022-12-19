using CookingBook.Shared.Abstractions.Commands;

namespace CookingBook.Application.Commands.Review;

public record AddReview(Guid RecipeId, string Name, string Content, short Rate) : ICommand;