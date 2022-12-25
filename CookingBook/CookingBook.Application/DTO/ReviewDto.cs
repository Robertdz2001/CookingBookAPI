namespace CookingBook.Application.DTO;

public class ReviewDto
{
    public string Name { get; set; }
    
    public DateTime CreatedDate { get; set; }
    
    public string Content { get; set; }
    
    public short Rate { get; set; }
    
    public UserDto User {get; set;}
}