using CookingBook.Shared.Abstractions.Commands;

namespace CookingBook.Application.Commands.Review;

public record RemoveReview(Guid RecipeId, string Name):ICommand;