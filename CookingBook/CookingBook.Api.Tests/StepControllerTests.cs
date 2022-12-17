namespace CookingBook.Api.Tests;

public class StepControllerTests: IClassFixture<WebApplicationFactory<Program>>
{
    #region POST_TESTS

    [Fact]
    public async Task
        Post_Returns_NotFound_When_There_Is_No_Recipe_With_Given_Id()
    {
        var httpContent = GetHttpContentForAddStepModel("Name");
        
        var response = await _client.PostAsync($"api/recipes/{Guid.NewGuid()}/steps", httpContent);
        
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task
        Post_Returns_Unauthorized_When_User_Is_Not_Author_Or_Admin()
    {
        var rid = Guid.NewGuid();
        
        var recipe = new Recipe(Guid.NewGuid(), rid, "Recipe", "Url", 39, DateTime.UtcNow);
        
        await _writeDbContext.AddAsync(recipe);
        await _writeDbContext.SaveChangesAsync();
        
        var httpContent = GetHttpContentForAddStepModel("Name");
        
        var response = await _client.PostAsync($"api/recipes/{rid}/steps", httpContent);
        
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task
        Post_Returns_BadRequest_When_Step_Name_Is_Empty()
    {
        var rid = Guid.NewGuid();
        
        var recipe = new Recipe(Guid.Parse("bb21ce33-ea66-4c56-aefc-5f8588f95766"), rid, "Recipe", "Url", 39, DateTime.UtcNow);
        
        await _writeDbContext.AddAsync(recipe);
        await _writeDbContext.SaveChangesAsync();
        
        var httpContent = GetHttpContentForAddStepModel("");
        
        var response = await _client.PostAsync($"api/recipes/{rid}/steps", httpContent);
        
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);


    }
    
    [Fact]
    public async Task
        Post_Returns_Created_On_Success()
    {
        var rid = Guid.NewGuid();
        
        var recipe = new Recipe(Guid.Parse("bb21ce33-ea66-4c56-aefc-5f8588f95766"), rid, "Recipe", "Url", 39, DateTime.UtcNow);
        
        await _writeDbContext.AddAsync(recipe);
        await _writeDbContext.SaveChangesAsync();
        
        var httpContent = GetHttpContentForAddStepModel("Name");
        
        var response = await _client.PostAsync($"api/recipes/{rid}/steps", httpContent);
        
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        response.Headers.Location.ShouldNotBeNull();
    
    }
    #endregion
    
    #region PUT_TESTS
    
    [Fact]
    public async Task
        Put_Returns_Ok_On_Success()
    {
        var recipe =
            GetUsersRecipeWithStep(Guid.Parse("bb21ce33-ea66-4c56-aefc-5f8588f95766"), "StepToChange");
        
        await _writeDbContext.AddAsync(recipe);
        await _writeDbContext.SaveChangesAsync();
        
        var httpContent = GetHttpContentForAddStepModel("Name");
        
        var response = await _client.PutAsync($"api/recipes/{(Guid)recipe.Id}/steps/StepToChange", httpContent);
        
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        
    }
    
    [Fact]
    public async Task
        Put_Returns_BadRequest_When_Step_Name_Is_Empty()
    {
        var recipe =
            GetUsersRecipeWithStep(Guid.Parse("bb21ce33-ea66-4c56-aefc-5f8588f95766"), "StepToChange");
        
        await _writeDbContext.AddAsync(recipe);
        await _writeDbContext.SaveChangesAsync();
        
        var httpContent = GetHttpContentForAddStepModel("");
        
        var response = await _client.PutAsync($"api/recipes/{(Guid)recipe.Id}/steps/StepToChange", httpContent);
        
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task
        Put_Returns_NotFound_When_There_Is_No_Recipe_With_Given_Id()
    {
        var httpContent = GetHttpContentForAddStepModel("Name");
        
        var response = await _client.PutAsync($"api/recipes/{Guid.NewGuid()}/steps/name", httpContent);
        
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        
    }
    
    [Fact]
    public async Task
        Put_Returns_NotFound_When_There_Is_No_Step_With_Given_Name()
    {
        var recipe =
            GetUsersRecipeWithStep(Guid.Parse("bb21ce33-ea66-4c56-aefc-5f8588f95766"), "StepToChange");
        
        await _writeDbContext.AddAsync(recipe);
        await _writeDbContext.SaveChangesAsync();
    
        var httpContent = GetHttpContentForAddStepModel("Name");
        
        var response = await _client.PutAsync($"api/recipes/{(Guid)recipe.Id}/steps/name", httpContent);
        
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        
    }
    
    [Fact]
    public async Task
        Put_Returns_Unauthorized_When_User_Is_Not_Author_Or_Admin()
    {
        var recipe = GetUsersRecipeWithStep(Guid.NewGuid(),"StepToChange");
        
        await _writeDbContext.AddAsync(recipe);
        await _writeDbContext.SaveChangesAsync();
    
        var httpContent = GetHttpContentForAddStepModel("Name");
        
        var response = await _client.PutAsync($"api/recipes/{(Guid)recipe.Id}/steps/StepToChange", httpContent);
        
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        
    }
     #endregion
    
    #region DELETE_TESTS
    
    [Fact]
    public async Task
        Delete_Returns_NoContent_OnSuccess()
    {
        var recipe = GetUsersRecipeWithStep(Guid.Parse("bb21ce33-ea66-4c56-aefc-5f8588f95766"),"StepToDelete");
        
        await _writeDbContext.AddAsync(recipe);
        await _writeDbContext.SaveChangesAsync();
    
        var response = await _client.DeleteAsync($"api/recipes/{(Guid)recipe.Id}/steps/StepToDelete");
        
        response.StatusCode.ShouldBe((HttpStatusCode.NoContent));
    
    
    }
    
    
    [Fact]
    public async Task
        Delete_Returns_NotFound_When_There_Is_No_Recipe_With_Given_Id()
    {
    
        var response = await _client.DeleteAsync($"api/recipes/{Guid.NewGuid()}/steps/StepToDelete");
    
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    
    }
    
    [Fact]
    public async Task
        Delete_Returns_NotFound_When_There_Is_No_Step_With_Given_Name()
    {
    
        var recipe = GetUsersRecipeWithStep(Guid.Parse("bb21ce33-ea66-4c56-aefc-5f8588f95766"), "Name");
        
        await _writeDbContext.AddAsync(recipe);
        await _writeDbContext.SaveChangesAsync();
        
        var response = await _client.DeleteAsync($"api/recipes/{(Guid)recipe.Id}/steps/StepToDelete");
    
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    
    }
    
    [Fact]
    public async Task
        Delete_Returns_Unauthorized_When_User_Is_Not_Author_Or_Admin()
    {
        var recipe = GetUsersRecipeWithStep(Guid.NewGuid(), "StepToDelete");
        
        await _writeDbContext.AddAsync(recipe);
        await _writeDbContext.SaveChangesAsync();
        
        var response = await _client.DeleteAsync($"api/recipes/{(Guid)recipe.Id}/steps/StepToDelete");
    
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        
        
    }
    #endregion
    
    #region ARRANGE
    private HttpClient _client;
    private WebApplicationFactory<Program> _factory;
    private ReadDbContext _readDbContext;
    private WriteDbContext _writeDbContext;
    
    private StringContent? GetHttpContentForAddStepModel(string name)
    {
        var model = new AddStepModel(name);

        var json = JsonConvert.SerializeObject(model);

        var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

        return httpContent;
    }
    
    
    private Recipe GetUsersRecipeWithStep(Guid userId, string stepName)
    {
    
        var recipe = new Recipe(userId, Guid.NewGuid(), "Name", "Url", 30,
            DateTime.UtcNow);
    
        var step = new Step(stepName);
        
        recipe.AddStep(step);
    
        return recipe;
    }
    
    public StepControllerTests(WebApplicationFactory<Program> factory)
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
    
    
    #endregion
}