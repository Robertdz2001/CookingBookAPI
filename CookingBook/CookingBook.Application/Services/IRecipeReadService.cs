namespace CookingBook.Application.Services;

public interface IRecipeReadService
{
    Task<bool> ExistsById(Guid id);
}