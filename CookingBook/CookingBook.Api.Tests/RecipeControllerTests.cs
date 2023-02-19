﻿using CookingBook.Application.DTO;

namespace CookingBook.Api.Tests;

public class RecipeControllerTests: IClassFixture<WebApplicationFactory<Program>>
{
    #region GET
    [Theory]
    [InlineData("?SearchPhrase=Recipe1&SortBy=3&SortByDescending=true&PageNumber=1&PageSize=20",1)]
    [InlineData("?SearchPhrase=Recipe&SortBy=3&SortByDescending=true&PageNumber=1&PageSize=20",2)]
    [InlineData("?SearchPhrase=Recipe3&SortBy=3&SortByDescending=true&PageNumber=1&PageSize=20",0)]
    [InlineData("?SortBy=3&SortByDescending=true&PageNumber=1&PageSize=1",1)]
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
    
       var responseRecipesList = JsonConvert.DeserializeObject<PagedResult<RecipeDto>>(responseJson);

       responseRecipesList.Items.Count.ShouldBe(recipesCount);
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
            User = new UserReadModel{Id = Guid.NewGuid(),ImageUrl = "UserUrl", PasswordHash = "123", UserName = "userName"},
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
        var response = await GetHttpPostResponseForRecipeModel("","Url",30);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        
    }
    
    [Fact]
    public async Task
        Post_Returns_Created_When_Model_Name_Is_Not_Empty()
    {
        var response = await GetHttpPostResponseForRecipeModel("Name","Url",30);

        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        response.Headers.Location.ShouldNotBeNull();

    }
    #endregion
    
    #region ARRANGE
    private HttpClient _client;
    private WebApplicationFactory<Program> _factory;
    private ReadDbContext _readDbContext;
    private WriteDbContext _writeDbContext;

    private async Task<HttpResponseMessage?> GetHttpPostResponseForRecipeModel(string name, string url, ushort prepTime)
    {
        var model = new CreateRecipeDto(name, url, prepTime);

        var json = JsonConvert.SerializeObject(model);

        var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
        
        var response = await _client.PostAsync("api/recipes", httpContent);

        return response;

    }
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

    private async Task InitDataBase()
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
                    User = new UserReadModel{Id = Guid.NewGuid(),ImageUrl = "UserUrl", PasswordHash = "123", UserName = "userName"},
                    CreatedDate = DateTime.UtcNow,
                    Id = Guid.NewGuid()
                },
                new RecipeReadModel()
                {
                    Name = "Recipe2",
                    ImageUrl = "Url2",
                    Calories = 0,
                    User = new UserReadModel{Id = Guid.NewGuid(),ImageUrl = "UserUrl", PasswordHash = "123", UserName = "userName"},
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