using System.Net;
using System.Text;
using CookingBook.Api.Models;
using CookingBook.Domain.Entities;
using CookingBook.Domain.ValueObjects;
using CookingBook.Infrastructure.EF.Contexts;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Shouldly;

namespace CookingBook.Api.Tests;

public class IngredientControllerTests: IClassFixture<WebApplicationFactory<Program>>
{
    private HttpClient _client;
    private WebApplicationFactory<Program> _factory;
    private ReadDbContext _readDbContext;
    private WriteDbContext _writeDbContext;
    
    
    public IngredientControllerTests(WebApplicationFactory<Program> factory)
    {
        
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var readDbContextOptions = services
                    .SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<ReadDbContext>));
                
                var writeDbContextOptions = services
                    .SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<WriteDbContext>));

                services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                services.AddMvc(option => option.Filters.Add(new FakeUserFilter()));
                
                services.Remove(readDbContextOptions);
                services.Remove(writeDbContextOptions);
                services.AddDbContext<ReadDbContext>(options => options.UseInMemoryDatabase("ReadDb"));
                services.AddDbContext<WriteDbContext>(options => options.UseInMemoryDatabase("WriteDb"));
            });
        });

        _client = _factory.CreateClient();
        
        
        var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();

        var scope = scopeFactory.CreateScope();

        _readDbContext = scope.ServiceProvider.GetService<ReadDbContext>();
        _writeDbContext = scope.ServiceProvider.GetService<WriteDbContext>();
    }


    [Fact]
    public async Task
        Post_Returns_NotFound_When_There_Is_No_Recipe_With_Given_Id()
    {
        var model = new AddIngredientModel("Name", 30, 30);
        
        var json = JsonConvert.SerializeObject(model);
        
        var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
        
        var response = await _client.PostAsync($"api/recipes/{Guid.NewGuid()}/ingredients", httpContent);
        
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        
    }
    
    [Fact]
    public async Task
        Post_Returns_Unauthorized_When_User_Is_Not_Author_Or_Admin()
    {
        var model = new AddIngredientModel("Name", 30, 30);

        var rid = Guid.NewGuid();
        
        var recipe = new Recipe(Guid.NewGuid(), rid, "Recipe", "Url", 39, DateTime.UtcNow);
        
        await _writeDbContext.AddAsync(recipe);
        await _writeDbContext.SaveChangesAsync();
        var json = JsonConvert.SerializeObject(model);
        
        var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
        
        var response = await _client.PostAsync($"api/recipes/{rid}/ingredients", httpContent);
        
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        
    }
    
    [Fact]
    public async Task
        Post_Returns_Created_On_Success()
    {
        var model = new AddIngredientModel("Name", 30, 30);

        var rid = Guid.NewGuid();
        
        var recipe = new Recipe(Guid.Parse("bb21ce33-ea66-4c56-aefc-5f8588f95766"), rid, "Recipe", "Url", 39, DateTime.UtcNow);
        
        await _writeDbContext.AddAsync(recipe);
        await _writeDbContext.SaveChangesAsync();
        var json = JsonConvert.SerializeObject(model);
        
        var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
        
        var response = await _client.PostAsync($"api/recipes/{rid}/ingredients", httpContent);
        
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        response.Headers.Location.ShouldNotBeNull();

    }

    [Fact]
    public async Task
        Put_Returns_Ok_On_Success()
    {
        var model = new AddIngredientModel("Name", 30, 30);

        var rid = Guid.NewGuid();
        
        var recipe = new Recipe(Guid.Parse("bb21ce33-ea66-4c56-aefc-5f8588f95766"), rid, "Recipe", "Url", 39, DateTime.UtcNow);

        var ingredientToChange = new Ingredient("IngredientToChange", 30, 50);
        
        recipe.AddIngredient(ingredientToChange);
        
        await _writeDbContext.AddAsync(recipe);
        await _writeDbContext.SaveChangesAsync();
        
        var json = JsonConvert.SerializeObject(model);
        
        var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
        
        var response = await _client.PutAsync($"api/recipes/{rid}/ingredients/{ingredientToChange.Name}", httpContent);
        
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        
    }
    
    [Fact]
    public async Task
        Put_Returns_NotFound_When_There_Is_No_Recipe_With_Given_Id()
    {
        var model = new AddIngredientModel("Name", 30, 30);

        var json = JsonConvert.SerializeObject(model);
        
        var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
        
        var response = await _client.PutAsync($"api/recipes/{Guid.NewGuid()}/ingredients/name", httpContent);
        
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        
    }
    
    [Fact]
    public async Task
        Put_Returns_NotFound_When_There_Is_No_Ingredient_With_Given_Name()
    {
        var model = new AddIngredientModel("Name", 30, 30);
        
        var rid = Guid.NewGuid();
        
        var recipe = new Recipe(Guid.Parse("bb21ce33-ea66-4c56-aefc-5f8588f95766"), rid, "Recipe", "Url", 39, DateTime.UtcNow);

        var ingredientToChange = new Ingredient("IngredientToChange", 30, 50);
        
        recipe.AddIngredient(ingredientToChange);
        
        await _writeDbContext.AddAsync(recipe);
        await _writeDbContext.SaveChangesAsync();

        var json = JsonConvert.SerializeObject(model);
        
        var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
        
        var response = await _client.PutAsync($"api/recipes/{rid}/ingredients/name", httpContent);
        
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        
    }
    
    [Fact]
    public async Task
        Put_Returns_Unauthorized_When_User_Is_Not_Author_Or_Admin()
    {
        var model = new AddIngredientModel("Name", 30, 30);
        
        var rid = Guid.NewGuid();
        
        var recipe = new Recipe(Guid.NewGuid(), rid, "Recipe", "Url", 39, DateTime.UtcNow);

        var ingredientToChange = new Ingredient("IngredientToChange", 30, 50);
        
        recipe.AddIngredient(ingredientToChange);
        
        await _writeDbContext.AddAsync(recipe);
        await _writeDbContext.SaveChangesAsync();

        var json = JsonConvert.SerializeObject(model);
        
        var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
        
        var response = await _client.PutAsync($"api/recipes/{rid}/ingredients/{ingredientToChange.Name}", httpContent);
        
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        
    }
    
    
    
    
    
}