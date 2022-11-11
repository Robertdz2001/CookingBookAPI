using CookingBook.Domain.Entities;
using CookingBook.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CookingBook.Infrastructure.EF.Configuration;

public class WriteConfiguration: IEntityTypeConfiguration<Recipe>, IEntityTypeConfiguration<Ingredient>
    , IEntityTypeConfiguration<Step>, IEntityTypeConfiguration<Tool>
{
    public void Configure(EntityTypeBuilder<Recipe> builder)
    {
        builder.ToTable("Recipes");
        builder.HasKey(r => r.Id);
        
        var recipeNameConverter = new ValueConverter<RecipeName, string>(rn => rn.Value,
            rn => new RecipeName(rn));
        
        var recipeImageUrlConverter = new ValueConverter<RecipeImageUrl, string>(ri => ri.Value,
            ri => new RecipeImageUrl(ri));
        
        var recipePrepTimeConverter = new ValueConverter<RecipePrepTime, ushort>(rp => rp.Value,
            rp => new RecipePrepTime(rp));
        
        var recipeCaloriesConverter = new ValueConverter<RecipeCalories, double>(rc => rc.Value,
            rc => new RecipeCalories(rc));
        
        var recipeCreatedDateConverter = new ValueConverter<RecipeCreatedDate, DateTime>(rcd => rcd.Value,
            rcd => new RecipeCreatedDate(rcd));
        
        builder
            .Property(r => r.Id)
            .HasConversion(id => id.Value, id => new RecipeId(id));
        
        builder
            .Property(typeof(RecipeName), "_name")
            .HasConversion(recipeNameConverter)
            .HasColumnName("Name");
        
        builder
            .Property(typeof(RecipeImageUrl), "_imageUrl")
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
        
        builder.HasMany(typeof(Ingredient), "_ingredients");
        
        builder.HasMany(typeof(Tool), "_tools");
        
        builder.HasMany(typeof(Step), "_steps");
        
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
}