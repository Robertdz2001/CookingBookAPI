namespace CookingBook.Application.DTO;

public class UserDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public short UserRating { get; set; }
    
    public int RoleId { get; set; }
    
    public string ImageUrl { get; set; }
    
}