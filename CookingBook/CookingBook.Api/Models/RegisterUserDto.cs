namespace CookingBook.Api.Models;

public record RegisterUserDto(string UserName, string Password,string ImageUrl, string ConfirmPassword);
