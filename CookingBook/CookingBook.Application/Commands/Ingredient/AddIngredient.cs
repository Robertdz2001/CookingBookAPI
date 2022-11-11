using CookingBook.Domain.ValueObjects;
using CookingBook.Shared.Abstractions.Commands;

namespace CookingBook.Application.Commands;

public record AddIngredient(Guid RecipeId, string Name, ushort Grams, ushort CaloriesPerHundredGrams) : ICommand;
