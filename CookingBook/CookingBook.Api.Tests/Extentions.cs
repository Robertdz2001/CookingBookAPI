namespace CookingBook.Api.Tests;

public static class Extentions
{
    public static StringContent? GetHttpContentForModel<T>(this T model)
    {
        
        var json = JsonConvert.SerializeObject(model);

        var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

        return httpContent;
    }
}