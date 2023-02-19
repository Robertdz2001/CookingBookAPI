using CookingBook.Domain.Entities;
using CookingBook.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CookingBook.Infrastructure.EF.Configuration;

public class WriteConfiguration:IEntityTypeConfiguration<User>, IEntityTypeConfiguration<Recipe>, IEntityTypeConfiguration<Ingredient>
    , IEntityTypeConfiguration<Step>, IEntityTypeConfiguration<Tool>, IEntityTypeConfiguration<Review>
{
    
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(u => u.Id);
        
        var userNameConverter = new ValueConverter<UserName, string>(un => un.Value,
            un => new UserName(un));
        
        var userPasswordHashConverter = new ValueConverter<PasswordHash, string>(p => p.Value,
            p => new PasswordHash(p));
        
        var userRatingConverter = new ValueConverter<UserRating, short>(ur => ur.Value,
            ur => new UserRating(ur));
        
        var userImageUrlConverter = new ValueConverter<ImageUrl, string>(ur => ur.Value,
            ur => new ImageUrl(ur));
        
        builder
            .Property(u => u.Id)
            .HasConversion(id => id.Value, id => new UserId(id));
        
        builder
            .Property(typeof(UserName), "_userName")
            .HasConversion(userNameConverter)
            .HasColumnName("UserName");
        
         builder
             .Property(u=>u.PasswordHash)
             .HasConversion(userPasswordHashConverter)
             .HasColumnName("PasswordHash");
         
         builder
             .Property(typeof(UserRating), "_userRating")
             .HasConversion(userRatingConverter)
             .HasColumnName("UserRating");
         
         builder
             .Property(typeof(ImageUrl), "_imageUrl")
             .HasConversion(userImageUrlConverter)
             .HasColumnName("ImageUrl");

         builder.HasOne(u => u.Role)
                .WithMany()
                .HasForeignKey(u=>u.RoleId);

        
        builder.HasMany(typeof(Recipe), "_recipes");
        
    }
    public void Configure(EntityTypeBuilder<Recipe> builder)
    {
        builder.ToTable("Recipes");
        builder.HasKey(r => r.Id);
        
        var recipeNameConverter = new ValueConverter<RecipeName, string>(rn => rn.Value,
            rn => new RecipeName(rn));
        
        var recipeImageUrlConverter = new ValueConverter<ImageUrl, string>(ri => ri.Value,
            ri => new ImageUrl(ri));
        
        var recipePrepTimeConverter = new ValueConverter<RecipePrepTime, ushort>(rp => rp.Value,
            rp => new RecipePrepTime(rp));
        
        var recipeCaloriesConverter = new ValueConverter<RecipeCalories, double>(rc => rc.Value,
            rc => new RecipeCalories(rc));
        
        var recipeCreatedDateConverter = new ValueConverter<RecipeCreatedDate, DateTime>(rcd => rcd.Value,
            rcd => new RecipeCreatedDate(rcd));
        
        var recipeRatingConverter = new ValueConverter<RecipeRating, short>(rr => rr.Value,
            rr => new RecipeRating(rr));
        
        builder
            .Property(r => r.Id)
            .HasConversion(id => id.Value, id => new RecipeId(id));
        
        builder.Property(r=>r.UserId)
            .HasConversion(id => id.Value, id => new UserId(id));
        
        builder
            .Property(typeof(RecipeName), "_name")
            .HasConversion(recipeNameConverter)
            .HasColumnName("Name");
        
        builder
            .Property(typeof(ImageUrl), "_imageUrl")
            .HasConversion(recipeImageUrlConverter)
            .HasColumnName("ImageUrl");
        
        builder
            .Property(typeof(RecipePrepTime), "_prepTime")
            .HasConversion(recipePrepTimeConverter)
            .HasColumnName("PrepTime");
        
        builder
            .Property(typeof(RecipeCalories), "_calories")
            .HasConversion(recipeCaloriesConverter)
            .HasColumnName("Calories");
        
        builder
            .Property(typeof(RecipeCreatedDate), "_createdDate")
            .HasConversion(recipeCreatedDateConverter)
            .HasColumnName("CreatedDate");
        
        builder
            .Property(typeof(RecipeRating), "_recipeRating")
            .HasConversion(recipeRatingConverter)
            .HasColumnName("RecipeRating");
        
        builder.HasMany(typeof(Ingredient), "_ingredients");
        
        builder.HasMany(typeof(Tool), "_tools");
        
        builder.HasMany(typeof(Step), "_steps");
        
        builder.HasMany(typeof(Review), "_reviews");
        
    }

    public void Configure(EntityTypeBuilder<Ingredient> builder)
    {
        builder.Property<Guid>("Id");
        builder.Property(i => i.Name);
        builder.Property(i => i.Grams);
        builder.Property(i => i.CaloriesPerHundredGrams);
        builder.Property(i=>i.RecipeId)
            .HasConversion(id => id.Value, id => new RecipeId(id));

        builder.ToTable("Ingredients");
    }

    public void Configure(EntityTypeBuilder<Step> builder)
    {
        builder.Property<Guid>("Id");
        builder.Property(s => s.Name);
        builder.Property(s=>s.RecipeId)
            .HasConversion(id => id.Value, id => new RecipeId(id));

        builder.ToTable("Steps");
    }

    public void Configure(EntityTypeBuilder<Tool> builder)
    {
        builder.Property<Guid>("Id");
        builder.Property(t => t.Name);
        builder.Property(t => t.Quantity);
        builder.Property(t=>t.RecipeId)
            .HasConversion(id => id.Value, id => new RecipeId(id));

        builder.ToTable("Tools");
    }


    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.Property<Guid>("Id");
        builder.Property(r => r.Name);
        builder.Property(r => r.CreatedDate);
        builder.Property(r => r.Content);
        builder.Property(r => r.Rate);
        builder.Property(r=>r.RecipeId)
            .HasConversion(id => id.Value, id => new RecipeId(id));
        builder.Property(r=>r.UserId)
            .HasConversion(id => id.Value, id => new UserId(id));

        builder.ToTable("Reviews");
    }
}