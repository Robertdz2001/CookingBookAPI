using CookingBook.Domain.ValueObjects;

namespace CookingBook.Domain.Entities;

public class User
{
    public UserId Id { get; private set; }

    private UserName _userName;

    private UserRating _userRating;

    private ImageUrl _imageUrl;
    public PasswordHash PasswordHash { get; set; }

    private LinkedList<Recipe> _recipes = new();

    public Role Role;
    
    public int RoleId;
    public User(UserId id, UserName userName,  PasswordHash passwordHash,ImageUrl imageUrl ,int roleId=1)
    {
        Id = id;
        _userName = userName;
        RoleId = roleId;
        PasswordHash = passwordHash;
        _imageUrl = imageUrl;
        _userRating = 0;
    }
    
    public User(){}

    public void AddRate(short rate)
    {
        _userRating += rate;
    }

    public void SetUserRole(int roleId)
    {
        RoleId = roleId;
    }
}