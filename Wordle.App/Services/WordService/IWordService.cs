namespace Wordle.App.Services.WordService;

public interface IWordService
{
    Task<string> GetWordAsync();
}