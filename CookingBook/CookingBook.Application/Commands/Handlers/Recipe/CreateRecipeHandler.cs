using CookingBook.Application.Exceptions;
using CookingBook.Application.Services;
using CookingBook.Domain;
using CookingBook.Domain.Factories;
using CookingBook.Shared.Abstractions.Commands;

namespace CookingBook.Application.Commands.Handlers;

public class CreateRecipeHandler : ICommandHandler<CreateRecipe>
{
    
    private readonly IRecipeFactory _factory;
    private readonly IRecipeRepository _repository;
    private readonly IRecipeReadService _readService;
    private readonly IUserContextService _userContext;

    public CreateRecipeHandler(IRecipeFactory factory, IRecipeRepository repository
        , IRecipeReadService readService, IUserRepository userRepository, IUserContextService userContext)
    {
        _factory = factory;
        _repository = repository;
        _readService = readService;
        _userContext = userContext;
    }
    public async Task HandleAsync(CreateRecipe command)
    {
        
        var alreadyExists = await _readService.ExistsById(command.Id);

        if (alreadyExists)
        {
            throw new RecipeAlreadyExistsException(command.Id);
        }

        var newRecipe = _factory.Create(_userContext.GetUserId,command.Id, command.Name, command.ImageUrl, command.PrepTime);

        await _repository.AddAsync(newRecipe);


    }
}