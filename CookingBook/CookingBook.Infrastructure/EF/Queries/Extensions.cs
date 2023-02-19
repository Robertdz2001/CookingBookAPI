using CookingBook.Application.DTO;
using CookingBook.Infrastructure.EF.Models;

namespace CookingBook.Infrastructure.EF.Queries;

public static class Extensions
{
    public static RecipeDto AsDto(this RecipeReadModel readModel)
        => new()
        {
            Id = readModel.Id,
            Name = readModel.Name,
            ImageUrl = readModel.ImageUrl,
            PrepTime = readModel.PrepTime,
            Calories = readModel.Calories,
            CreatedDate = readModel.CreatedDate,
            RecipeRating = readModel.RecipeRating,

            Tools = readModel.Tools.Select(t => new ToolDto
            {
                Name = t.Name,
                Quantity = t.Quantity
            }),
            
            Steps = readModel.Steps.Select(s => new StepDto()
            {
                Name = s.Name
            }),
            
            Ingredients = readModel.Ingredients.Select(i=>new IngredientDto
            {
                Name = i.Name,
                Grams = i.Grams,
                CaloriesPerHundredGrams = i.CaloriesPerHundredGrams
            }),
            
            Reviews = readModel.Reviews.Select(r=>new ReviewDto
            {
                Name = r.Name,
                Content = r.Content,
                CreatedDate = r.CreatedDate,
                Rate = r.Rate,
                User = new UserDto
                {
                    Id = r.User.Id,
                    UserName = r.User.UserName,
                    UserRating = r.User.UserRating
                }
            })

        };

    public static UserDto AsDto(this UserReadModel readModel)
        => new()
        {
            Id = readModel.Id,
            UserName = readModel.UserName,
            UserRating = readModel.UserRating
        };
}