using System.Net;
using System.Text;
using CookingBook.Api.Models;
using CookingBook.Domain.Entities;
using CookingBook.Infrastructure.EF.Contexts;
using CookingBook.Infrastructure.EF.Models;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Shouldly;


namespace CookingBook.Api.Tests;

public class RecipeControllerTests: IClassFixture<WebApplicationFactory<Program>>
{
    #region GET
    [Theory]
    [InlineData("?SearchPhrase=Recipe1",1)]
    [InlineData("?SearchPhrase=Recipe",2)]
    [InlineData("?SearchPhrase=Recipe3",0)]
    [InlineData("",2)]
    public async Task
    GetRecipes_Returns_IEnumerable_Of_Recipes_On_Success(string searchString,int recipesCount)
    {
        if (!_readDbContext.Recipes.Any())
        {
            await InitDataBase();
        }

        var response = await _client.GetAsync("/api/recipes"+searchString);

       var responseJson = await response.Content.ReadAsStringAsync();
    
       var responseRecipesList = JsonConvert.DeserializeObject<List<RecipeReadModel>>(responseJson);

       responseRecipesList.Count.ShouldBe(recipesCount);
       response.StatusCode.ShouldBe(HttpStatusCode.OK);

    }

    [Fact]
    public async Task
        GetRecipe_Returns_Recipe_OnSuccess()
    {
        var id = Guid.NewGuid();

       var recipe = new RecipeReadModel()
        {
            Name = "Recipe123",
            ImageUrl = "Url1",
            Calories = 0,
            CreatedDate = DateTime.UtcNow,
            Id = id
        };
        
        await _readDbContext.Recipes.AddAsync(recipe);
        await _readDbContext.SaveChangesAsync();

        var response = await _client.GetAsync("/api/recipes/"+id);

        var responseJson = await response.Content.ReadAsStringAsync();
    
        var responseRecipe = JsonConvert.DeserializeObject<RecipeReadModel>(responseJson);

        responseRecipe.Name.ShouldBe("Recipe123");
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Fact]
    public async Task
    GetRecipe_Returns_NotFound_When_There_Is_No_Recipe_With_Given_Id()
    {
        var response = await _client.GetAsync("/api/recipes/"+Guid.NewGuid());

        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task
        GetUserRecipes_Returns_IEnumerable_Of_Recipes_On_Success()
    {
        if (!_readDbContext.Recipes.Any())
        {
            await InitDataBase();
        }
        
        var recipes = new List<RecipeReadModel>()
        {
            new RecipeReadModel()
            {
                Name = "Recipe1",
                ImageUrl = "Url1",
                Calories = 0,
                CreatedDate = DateTime.UtcNow,
                Id = Guid.NewGuid(),
                UserId = Guid.Parse("bb21ce33-ea66-4c56-aefc-5f8588f95766")
            },
            new RecipeReadModel()
            {
                Name = "Recipe2",
                ImageUrl = "Url2",
                Calories = 0,
                CreatedDate = DateTime.UtcNow,
                Id = Guid.NewGuid(),
                UserId = Guid.Parse("bb21ce33-ea66-4c56-aefc-5f8588f95766")
                
            },
            new RecipeReadModel()
            {
                Name = "Recipe3",
                ImageUrl = "Url2",
                Calories = 0,
                CreatedDate = DateTime.UtcNow,
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid()
                
            }
            
        };

        await _readDbContext.Recipes.AddRangeAsync(recipes);
        await _readDbContext.SaveChangesAsync();
        
        

        var response = await _client.GetAsync("/api/recipes/user");

        var responseJson = await response.Content.ReadAsStringAsync();
    
        var responseRecipesList = JsonConvert.DeserializeObject<List<RecipeReadModel>>(responseJson);

        responseRecipesList.Count.ShouldBe(2);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }
    #endregion
    
    #region DELETE
    [Fact]
    public async Task
        Delete_Returns_NoContent_On_Success()
    {
        
        var recipe = new Recipe(Guid.Parse("bb21ce33-ea66-4c56-aefc-5f8588f95766"),Guid.NewGuid(),"Recipe123","Url",30,DateTime.UtcNow);
        
        await _writeDbContext.Recipes.AddAsync(recipe);
        await _writeDbContext.SaveChangesAsync();
        
        var response = await _client.DeleteAsync("/api/recipes/"+(Guid)recipe.Id);
    
    
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        
    }
    
    [Fact]
    public async Task
        Delete_Returns_NotFound_When_There_Is_No_Recipe_With_Given_Id()
    {

        var response = await _client.DeleteAsync("/api/recipes/"+Guid.NewGuid());
    
    
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        
    }
    
    
    [Fact]
    public async Task
        Delete_Returns_Forbid_When_User_Is_Not_Author_Or_Admin()
    {
        
        var recipe = new Recipe(Guid.NewGuid(),Guid.NewGuid(),"Recipe123","Url",30,DateTime.UtcNow);
        
        await _writeDbContext.Recipes.AddAsync(recipe);
        await _writeDbContext.SaveChangesAsync();
        
        var response = await _client.DeleteAsync("/api/recipes/"+(Guid)recipe.Id);
    
    
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        
    }
    #endregion
    
    #region POST

    [Fact]
    public async Task
        Post_Returns_BadRequest_When_Model_Name_Is_Empty()
    {
        var model = new CreateRecipeDto("", "Url", 30);

        var json = JsonConvert.SerializeObject(model);

        var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

        var response = await _client.PostAsync("api/recipes", httpContent);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        
    }
    
    [Fact]
    public async Task
        Post_Returns_Created_When_Model_Name_Is_Not_Empty()
    {
        var model = new CreateRecipeDto("Name", "Url", 30);

        var json = JsonConvert.SerializeObject(model);

        var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

        var response = await _client.PostAsync("api/recipes", httpContent);

        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        response.Headers.Location.ShouldNotBeNull();

    }
    #endregion
    
    #region ARRANGE
    private HttpClient _client;
    private WebApplicationFactory<Program> _factory;
    private ReadDbContext _readDbContext;
    private WriteDbContext _writeDbContext;
    public RecipeControllerTests(WebApplicationFactory<Program> factory)
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
    public async Task InitDataBase()
    {
        if (!_readDbContext.Recipes.Any())
        {
            var recipes = new List<RecipeReadModel>()
            {
                new RecipeReadModel()
                {
                    Name = "Recipe1",
                    ImageUrl = "Url1",
                    Calories = 0,
                    CreatedDate = DateTime.UtcNow,
                    Id = Guid.NewGuid()
                },
                new RecipeReadModel()
                {
                    Name = "Recipe2",
                    ImageUrl = "Url2",
                    Calories = 0,
                    CreatedDate = DateTime.UtcNow,
                    Id = Guid.NewGuid()
                }
            };

            await _readDbContext.Recipes.AddRangeAsync(recipes);
            await _readDbContext.SaveChangesAsync();
        }
    }

    #endregion

}