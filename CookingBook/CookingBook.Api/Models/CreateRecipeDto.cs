namespace CookingBook.Api.Models;

public record CreateRecipeDto(string Name, string ImageUrl, ushort PrepTime);
