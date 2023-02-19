using CookingBook.Domain.Entities;
using CookingBook.Infrastructure.EF.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CookingBook.Infrastructure.EF.Configuration;

internal sealed class ReadConfiguration: IEntityTypeConfiguration<Role>,IEntityTypeConfiguration<UserReadModel> ,IEntityTypeConfiguration<RecipeReadModel>, IEntityTypeConfiguration<IngredientReadModel>
    , IEntityTypeConfiguration<StepReadModel>, IEntityTypeConfiguration<ToolReadModel>,IEntityTypeConfiguration<ReviewReadModel>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");
        builder.HasData(GetRoles());
    }
    public void Configure(EntityTypeBuilder<UserReadModel> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(u => u.Id);
        
        builder
            .HasMany(u => u.Recipes)
            .WithOne(u => u.User);
        
        builder.HasOne(u => u.Role)
               .WithMany()
               .HasForeignKey(u=>u.RoleId);

        builder.HasData(GetUsers());
    }
    public void Configure(EntityTypeBuilder<RecipeReadModel> builder)
    {
        builder.ToTable("Recipes");
        builder.HasKey(r => r.Id);

        builder
            .HasMany(r => r.Ingredients)
            .WithOne(r => r.Recipe);
    
        builder
            .HasMany(r => r.Steps)
            .WithOne(r => r.Recipe);
        
        builder
            .HasMany(r => r.Tools)
            .WithOne(r => r.Recipe);
        
        builder.HasData(GetRecipes());

    }

    public void Configure(EntityTypeBuilder<IngredientReadModel> builder)
    {
        builder.ToTable("Ingredients");
        builder.HasData(GetIngredients());
    }

    public void Configure(EntityTypeBuilder<StepReadModel> builder)
    {
        builder.ToTable("Steps");
        builder.HasData(GetSteps());
    }

    public void Configure(EntityTypeBuilder<ToolReadModel> builder)
    {
        builder.ToTable("Tools");
        builder.HasData(GetTools());
    }
    
    public void Configure(EntityTypeBuilder<ReviewReadModel> builder)
    {
        builder.ToTable("Reviews");
    }
    
    
    
    private readonly Guid uid = Guid.NewGuid();
    private readonly Guid rid = Guid.NewGuid();
    private IEnumerable<UserReadModel> GetUsers()
    {
        
        var users = new List<UserReadModel>
        {
             new UserReadModel()
             {
                 Id = uid,
                 UserName = "Bill",
                 PasswordHash = "AQAAAAIAAYagAAAAEJ9Izg7Vu9QS7EbdzZZOWpf2B3ubMdSV7VbYwcL3apdXoXg9/N9uOlxH1K20XOz4BQ==",//12345
                 ImageUrl = "https://cdn-icons-png.flaticon.com/512/149/149071.png", 
                 RoleId = 2,

             },
             new UserReadModel()
             {
                 Id = Guid.NewGuid(),
                 UserName = "John",
                 PasswordHash = "AQAAAAIAAYagAAAAEJ9Izg7Vu9QS7EbdzZZOWpf2B3ubMdSV7VbYwcL3apdXoXg9/N9uOlxH1K20XOz4BQ==",
                 ImageUrl = "https://e7.pngegg.com/pngimages/178/595/png-clipart-user-profile-computer-icons-login-user-avatars-monochrome-black.png",
                 RoleId = 1,
                 Recipes = new List<RecipeReadModel>()
             }
     };

        return users;
    }
    
    private IEnumerable<Role> GetRoles()
    {
        
        var roles = new List<Role>
        {
            new Role()
            {
                Id = 1,
                Name = "User"

            },
            new Role()
            {
                Id = 2,
                Name = "Admin"
            }
        };

        return roles;
    }

    private IEnumerable<RecipeReadModel> GetRecipes()
    {
        var recipes = new List<RecipeReadModel>()
        {
            new RecipeReadModel()
            {
                Id = rid,
                UserId = uid,
                Version = 0,
                Name = "Test Recipe",
                Calories = 100,
                ImageUrl = "http://www.test.com",
                PrepTime = 50,
                CreatedDate = DateTime.UtcNow,
            }

        };

        return recipes;

    }

    private IEnumerable<IngredientReadModel> GetIngredients()
    {
        var ingredients = new List<IngredientReadModel>()
        {
            new IngredientReadModel()
            {
                Id = Guid.NewGuid(),
                RecipeId = rid,
                CaloriesPerHundredGrams = 100,
                Grams = 100,
                Name = "Test Ingredient"
            }

        };

        return ingredients;
    }

    private IEnumerable<ToolReadModel> GetTools()
    {
        var tools = new List<ToolReadModel>()
        {
            new ToolReadModel()
            {
                Id = Guid.NewGuid(),
                RecipeId = rid,
                Name = "Test Tool",
                Quantity = 5
            },
            new ToolReadModel()
            {
                Id = Guid.NewGuid(),
                RecipeId = rid,
                Name = "Test Tool2",
                Quantity = 3
            }

        };

        return tools;
    }

    private IEnumerable<StepReadModel> GetSteps()
    {
        var steps = new List<StepReadModel>()
        {
            new StepReadModel()
            {
                RecipeId = rid,
                Id = Guid.NewGuid(),
                Name = "Step1"
            },
            new StepReadModel()
            {
                RecipeId = rid,
                Id = Guid.NewGuid(),
                Name = "Step2"
            }

        };

        return steps;
    }
}

