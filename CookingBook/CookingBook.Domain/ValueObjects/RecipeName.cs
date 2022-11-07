using CookingBook.Domain.Exceptions;

namespace CookingBook.Domain.ValueObjects;

public record RecipeName
{
   public string Value { get; }

   public RecipeName(string value)
   {
      if (string.IsNullOrWhiteSpace(value))
      {
         throw new EmptyRecipeNameException();
      }
      
      Value = value;
   }

   public static implicit operator string(RecipeName name)
      => name.Value;
   
   public static implicit operator RecipeName(string name)
      => new(name);
}