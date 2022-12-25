using CookingBook.Domain.Exceptions;

namespace CookingBook.Domain.ValueObjects;

public class Review
{
    public string Name { get; set; }
    public DateTime CreatedDate { get; set; }
    public string Content { get; set; }
    public short Rate { get; set; }
    public RecipeId RecipeId { get; set; }
    public UserId UserId { get; set; }

    public Review(string name, string content, short rate ,Guid userId)
    {
        if (String.IsNullOrWhiteSpace(name))
        {
            throw new EmptyReviewNameException();
        }
        if (String.IsNullOrWhiteSpace(content))
        {
            throw new EmptyReviewContentException();
        }

        if (rate < -5 || rate > 5)
        {
            throw new InvalidReviewRateException();
        }

        Name = name;
        CreatedDate = DateTime.UtcNow;
        Content = content;
        Rate = rate;
        UserId = userId;

    }
    public Review(){}
    
}