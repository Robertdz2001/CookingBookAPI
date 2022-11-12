using CookingBook.Infrastructure.EF.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CookingBook.Infrastructure.EF.Configuration;

internal sealed class ReadConfiguration:IEntityTypeConfiguration<UserReadModel> ,IEntityTypeConfiguration<RecipeReadModel>, IEntityTypeConfiguration<IngredientReadModel>
    , IEntityTypeConfiguration<StepReadModel>, IEntityTypeConfiguration<ToolReadModel>
{
    
    public void Configure(EntityTypeBuilder<UserReadModel> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(u => u.Id);
        
        builder
            .HasMany(u => u.Recipes)
            .WithOne(u => u.User);
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

    }

    public void Configure(EntityTypeBuilder<IngredientReadModel> builder)
    {
        builder.ToTable("Ingredients");
    }

    public void Configure(EntityTypeBuilder<StepReadModel> builder)
    {
        builder.ToTable("Steps");
    }

    public void Configure(EntityTypeBuilder<ToolReadModel> builder)
    {
        builder.ToTable("Tools");
    }

   
}