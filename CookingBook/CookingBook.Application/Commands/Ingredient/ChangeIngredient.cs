using CookingBook.Shared.Abstractions.Commands;

namespace CookingBook.Application.Commands;

public record ChangeIngredient(Guid RecipeId, string IngredientToChangeName, string Name, ushort Grams,
    ushort CaloriesPerHundredGrams): ICommand ;
