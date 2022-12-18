using System.Linq.Expressions;
using CookingBook.Domain;
using CookingBook.Domain.Entities;
using CookingBook.Domain.ValueObjects;
using CookingBook.Infrastructure.EF.Contexts;
using CookingBook.Infrastructure.EF.Repositories;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using NSubstitute;
using Shouldly;

namespace CookingBook.Infrastructure.Tests;

public class PostgresRecipeRepositoryTests
{
    [Fact]
    public async Task
        GetAsync_Returns_Null_When_There_Is_No_Recipe_With_Given_Id()
    {
        var id = new RecipeId(Guid.NewGuid());
        
        var dbContextMock = new Mock<WriteDbContext>();  
        var dbSetMock = new Mock<DbSet<Recipe>>();  
        dbSetMock.Setup(s => s.FirstOrDefaultAsync(It.IsAny<Expression<Func<Recipe, bool>>>(),CancellationToken.None)).Returns(Task.FromResult(default(Recipe)));  
        dbContextMock.Setup(s => s.Set<Recipe>()).Returns(dbSetMock.Object);  
        
        
        var repository = new PostgresRecipeRepository(dbContextMock.Object);  
        
        var recipe = repository.GetAsync(Guid.NewGuid()).Result;  
        
        recipe.ShouldBeNull();
        
        // var id = new RecipeId(Guid.NewGuid());
        //
        // var recipe = new Recipe(Guid.NewGuid(),Guid.NewGuid(),"Name","Url",32,DateTime.UtcNow);
        //
        // _recipes.FirstOrDefaultAsync(u => u.Id == id).Returns(default(Recipe));
        //
        // var exception = await Record.ExceptionAsync(async () => recipe = await _repository.GetAsync(id));
        //
        // exception.ShouldBeNull(); 
        //
        // recipe.ShouldBeNull();



    }
    
    
    
    
    
    
    // private readonly DbSet<Recipe> _recipes;
    // private readonly WriteDbContext _writeDbContext;
    // private readonly IRecipeRepository _repository;
    //
    // public PostgresRecipeRepositoryTests()
    // {
    //     var factory = new WebApplicationFactory<Program>();
    //     factory.WithWebHostBuilder(builder =>
    //     {
    //         builder.ConfigureServices(services =>
    //         {
    //             var dbContextOptions = services
    //                 .SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<WriteDbContext>));
    //
    //             services.Remove(dbContextOptions);
    //             services.AddDbContext<WriteDbContext>(options => options.UseInMemoryDatabase("WriteDb"));
    //         });
    //     });
    //
    //     _writeDbContext = new WriteDbContext()
    //     _repository = new PostgresRecipeRepository(_writeDbContext);
    // }
    //
}