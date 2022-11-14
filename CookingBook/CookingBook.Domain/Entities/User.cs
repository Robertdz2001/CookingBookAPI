using CookingBook.Domain.Consts;
using CookingBook.Domain.ValueObjects;

namespace CookingBook.Domain.Entities;

public class User
{
    public UserId Id { get; private set; }

    private UserName _userName;

    public PasswordHash PasswordHash { get; set; }

    private LinkedList<Recipe> _recipes = new();

    private Role _userRole;
    public User(UserId id, UserName userName,  PasswordHash passwordHash,Role userRole=0)
    {
        Id = id;
        _userName = userName;
        _userRole = userRole;
        PasswordHash = "girnfgoinfd";
    }

    public User()
    {
        
    }

    public void SetUserRole(Role role)
    {
        _userRole = role;
    }
}