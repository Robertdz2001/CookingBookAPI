using CookingBook.Domain.Exceptions;

namespace CookingBook.Domain.ValueObjects;

public class Review
{
    public string Name { get; set; }
    public DateTime CreatedDate { get; set; }
    public string Content { get; set; }
    public short Rate { get; set; }
    
    
    public Guid RecipeId { get; set; }
    public Guid UserId { get; set; }

    public Review(string name, string content, short rate)
    {
        if (String.IsNullOrWhiteSpace(name))
        {
            throw new EmptyReviewNameException();
        }
        if (String.IsNullOrWhiteSpace(content))
        {
            throw new EmptyReviewContentException();
        }

        if (rate <= -5 || rate >= 5)
        {
            throw new InvalidReviewRateException();
        }

        Name = name;
        CreatedDate = DateTime.UtcNow;
        Content = content;
        Rate = rate;

    }
    
}