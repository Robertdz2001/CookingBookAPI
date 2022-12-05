namespace CookingBook.Api.Tests;

public class UserControllerTests: IClassFixture<WebApplicationFactory<Program>>
{
    #region CHANGE_ROLE

    [Fact]
    public async Task
        ChangeRole_Returns_NotFound_When_There_Is_No_User_With_Given_Id()
    {
        var role = Role.User;
        
        var json = JsonConvert.SerializeObject(role);
        
        var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
        
        var response = await _client.PutAsync($"api/user/{Guid.NewGuid()}/role", httpContent);
        
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);


    }
    [Fact]
    public async Task
        ChangeRole_Returns_NoContent_On_Success()
    {
        var user = new User(Guid.NewGuid(), "Name", "Password");
        await _writeDbContext.Users.AddAsync(user);
        await _writeDbContext.SaveChangesAsync();
        var role = Role.Admin;
        
        var json = JsonConvert.SerializeObject(role);
        
        var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
        
        var response = await _client.PutAsync($"api/user/{(Guid)user.Id}/role", httpContent);
        
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);


    }
    #endregion

    #region REGISTER
    
    [Fact]
    public async Task
        RegisterUser_Returns_Ok_For_Valid_Model()
    {
        var httpContent = GetHttpContentForRegisterModel("UserName123", "Password", "Password");

        var response = await _client.PostAsync("api/user/register", httpContent);
        
        response.StatusCode.ShouldBe(HttpStatusCode.OK);


    }
    
    [Theory]
    [InlineData("","Password","Password")]
    [InlineData("UserName","Password","Password1")]
    public async Task
        RegisterUser_Returns_BadRequest_When_Model_Is_Invalid(string userName, string password, string confirmPassword)
    {
        var httpContent = GetHttpContentForRegisterModel(userName, password, confirmPassword);

        var response = await _client.PostAsync("api/user/register", httpContent);
        
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);


    }

    
    #endregion

    #region LOGIN
    
    [Fact]
    public async Task
        Login_Returns_Ok_With_Token_On_Success()
    {
        var httpContent = await SeedDatabaseAndGetHttpContentForLogin("UserName"
            , "AQAAAAIAAYagAAAAEJ9Izg7Vu9QS7EbdzZZOWpf2B3ubMdSV7VbYwcL3apdXoXg9/N9uOlxH1K20XOz4BQ==" //12345
            ,"UserName"
            ,"12345");

        var response = await _client.PostAsync("api/user/login", httpContent);
        
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

    }
    
    
    [Theory]
    [InlineData("user","12345")]
    [InlineData("User","123456")]
    public async Task
        Login_Returns_BadRequest_When_UserName_Or_Password_Is_Invalid(string userName, string password)
    {
        var httpContent = await SeedDatabaseAndGetHttpContentForLogin("User"
            , "AQAAAAIAAYagAAAAEJ9Izg7Vu9QS7EbdzZZOWpf2B3ubMdSV7VbYwcL3apdXoXg9/N9uOlxH1K20XOz4BQ==" //12345
            ,userName
            ,password);

        var response = await _client.PostAsync("api/user/login", httpContent);
        
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

    }
    

    #endregion

    #region ARRANGE
    private HttpClient _client;
    private WebApplicationFactory<Program> _factory;
    private ReadDbContext _readDbContext;
    private WriteDbContext _writeDbContext;

    private async Task<StringContent?> SeedDatabaseAndGetHttpContentForLogin(string userNameToSeed, string passwordToSeed, string userNameDto,string passwordDto)
    {
        var newUser = new UserReadModel
        {
            UserName = userNameToSeed,
            PasswordHash = passwordToSeed,
        
        }; 
        
        await _readDbContext.Users.AddAsync(newUser);
        await _readDbContext.SaveChangesAsync();
     
        var loginDto = new LoginDto(userNameDto, passwordDto);

        var json = JsonConvert.SerializeObject(loginDto);

        return new StringContent(json, UnicodeEncoding.UTF8, "application/json");
    }
    private StringContent? GetHttpContentForRegisterModel(string userName,string password, string confirmPassword)
    {
        var registerModel = new RegisterUserDto(userName, password, confirmPassword);

        var json = JsonConvert.SerializeObject(registerModel);

        return new StringContent(json, UnicodeEncoding.UTF8, "application/json");
    }
    
    public UserControllerTests(WebApplicationFactory<Program> factory)
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