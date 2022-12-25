using CookingBook.Domain.Entities;
using CookingBook.Domain.ValueObjects;
using CookingBook.Infrastructure.EF.Configuration;
using Microsoft.EntityFrameworkCore;

namespace CookingBook.Infrastructure.EF.Contexts;

public class WriteDbContext : DbContext
{
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<User> Users { get; set; }
    
    public DbSet<Role> Roles { get; set; }
    public WriteDbContext(DbContextOptions<WriteDbContext> options) : base(options){}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("cookingBook");

        var configuration = new WriteConfiguration();
        
        modelBuilder.ApplyConfiguration<User>(configuration);

        modelBuilder.ApplyConfiguration<Recipe>(configuration);

        modelBuilder.ApplyConfiguration<Ingredient>(configuration);
        
        modelBuilder.ApplyConfiguration<Tool>(configuration);
        
        modelBuilder.ApplyConfiguration<Step>(configuration);
        
        modelBuilder.ApplyConfiguration<Review>(configuration);
    }
}