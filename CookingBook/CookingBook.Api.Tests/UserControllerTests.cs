using System.Net;
using System.Text;
using CookingBook.Domain.Consts;
using CookingBook.Domain.Entities;
using CookingBook.Infrastructure.EF.Contexts;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Shouldly;

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
    
    #region ARRANGE
    private HttpClient _client;
    private WebApplicationFactory<Program> _factory;
    private ReadDbContext _readDbContext;
    private WriteDbContext _writeDbContext;

    
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