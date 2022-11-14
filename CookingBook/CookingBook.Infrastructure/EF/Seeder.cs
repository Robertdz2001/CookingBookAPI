using CookingBook.Domain.Consts;
using CookingBook.Domain.Entities;
using CookingBook.Domain.ValueObjects;
using CookingBook.Infrastructure.EF.Contexts;
using CookingBook.Infrastructure.EF.Models;

namespace CookingBook.Infrastructure.EF;

public class Seeder
{
    private readonly ReadDbContext _context;
    public Seeder(ReadDbContext context)
    {
        _context = context;
    }


    public void Seed()
    {
        if (_context.Database.CanConnect())
        {

            if (!_context.Users.Any())
            {
                var users = GetUsers();
                _context.Users.AddRange(users);
                _context.SaveChanges();
            }
        }
    }

    private IEnumerable<UserReadModel> GetUsers()
    {
        var users = new List<UserReadModel>
        {
            new UserReadModel()
            {
                Id = Guid.NewGuid(),
                UserName = "Bill",
                PasswordHash = "AQAAAAIAAYagAAAAEJ9Izg7Vu9QS7EbdzZZOWpf2B3ubMdSV7VbYwcL3apdXoXg9/N9uOlxH1K20XOz4BQ==", //12345
                UserRole = Role.Admin,
                Recipes = new System.Collections.Generic.List<RecipeReadModel>()
                {
                    new RecipeReadModel()
                    {
                        Id = Guid.NewGuid(),
                        Version = 0,
                        Name = "Test Recipe",
                        Calories = 100,
                        ImageUrl = "http://www.test.com",
                        PrepTime = 50,
                        CreatedDate = DateTime.UtcNow,
                        Tools = new System.Collections.Generic.List<ToolReadModel>()
                        {
                            new ToolReadModel()
                            {
                                Id = Guid.NewGuid(),
                                Name = "Test Tool",
                                Quantity = 5
                            },
                            new ToolReadModel()
                            {
                            Id = Guid.NewGuid(),
                            Name = "Test Tool2",
                            Quantity = 3
                        }
                            
                        },
                        
                        Ingredients = new List<IngredientReadModel>()
                        {
                            new IngredientReadModel()
                            {
                                Id = Guid.NewGuid(),
                                CaloriesPerHundredGrams = 100,
                                Grams = 100,
                                Name = "Test Ingredient"
                            }
                        },
                        
                        Steps = new List<StepReadModel>()
                        {
                            new StepReadModel()
                            {
                                Id = Guid.NewGuid(),
                                Name = "Step1"
                            },
                            new StepReadModel()
                            {
                                Id = Guid.NewGuid(),
                                Name = "Step2"
                            }
                        }
                    }
                }
            },
            
            new UserReadModel()
            {
                Id = Guid.NewGuid(),
                UserName = "John",
                PasswordHash = "AQAAAAIAAYagAAAAEJ9Izg7Vu9QS7EbdzZZOWpf2B3ubMdSV7VbYwcL3apdXoXg9/N9uOlxH1K20XOz4BQ==",
                UserRole = Role.User,
                Recipes = new List<RecipeReadModel>()
            }
    };

        return users;
    }
}