namespace CookingBook.Application.Services;

public interface IUserReadService
{
    Task<bool> ExistsByUserName(string userName);
}