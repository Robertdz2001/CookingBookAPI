using CookingBook.Domain.Entities;
using CookingBook.Domain.ValueObjects;
using CookingBook.Infrastructure.EF.Configuration;
using Microsoft.EntityFrameworkCore;

namespace CookingBook.Infrastructure.EF.Contexts;

public class WriteDbContext : DbContext
{
    public DbSet<Recipe> Recipes { get; set; }
    
    public WriteDbContext(DbContextOptions<WriteDbContext> options) : base(options){}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("cookingBook");

        var configuration = new WriteConfiguration();

        modelBuilder.ApplyConfiguration<Recipe>(configuration);

        modelBuilder.ApplyConfiguration<Ingredient>(configuration);
        
        modelBuilder.ApplyConfiguration<Tool>(configuration);
        
        modelBuilder.ApplyConfiguration<Step>(configuration);
    }
}