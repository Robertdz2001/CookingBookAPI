using CookingBook.Domain.Entities;
using CookingBook.Infrastructure.EF.Configuration;
using CookingBook.Infrastructure.EF.Models;
using Microsoft.EntityFrameworkCore;
namespace CookingBook.Infrastructure.EF.Contexts;

public class ReadDbContext : DbContext
{
    
    
    public DbSet<RecipeReadModel> Recipes { get; set; }
    public DbSet<UserReadModel> Users { get; set; }

    public DbSet<Role> Roles { get; set; }
    public ReadDbContext(DbContextOptions<ReadDbContext> options) : base(options){}
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("cookingBook");

        var configuration = new ReadConfiguration();
        
        modelBuilder.ApplyConfiguration<Role>(configuration);
        
        modelBuilder.ApplyConfiguration<UserReadModel>(configuration);

        modelBuilder.ApplyConfiguration<RecipeReadModel>(configuration);

        modelBuilder.ApplyConfiguration<IngredientReadModel>(configuration);
    
        modelBuilder.ApplyConfiguration<StepReadModel>(configuration);
        
        modelBuilder.ApplyConfiguration<ToolReadModel>(configuration);
        
        modelBuilder.ApplyConfiguration<ReviewReadModel>(configuration);

    }
    
}