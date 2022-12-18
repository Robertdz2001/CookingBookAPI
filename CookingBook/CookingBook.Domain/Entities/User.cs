using CookingBook.Domain.ValueObjects;

namespace CookingBook.Domain.Entities;

public class User
{
    public UserId Id { get; private set; }

    private UserName _userName;

    public PasswordHash PasswordHash { get; set; }

    private LinkedList<Recipe> _recipes = new();

    public Role Role;
    
    public int RoleId;
    public User(UserId id, UserName userName,  PasswordHash passwordHash,int roleId=1)
    {
        Id = id;
        _userName = userName;
        RoleId = roleId;
        PasswordHash = passwordHash;
    }
    
    public User(){}

    public void SetUserRole(int roleId)
    {
        RoleId = roleId;
    }
}