namespace Sonab.WebAPI.Models;

public class ErrorMessages
{
    public Dictionary<string, string[]> Errors { get; set; }

    public ErrorMessages(string key, params string[] errors)
    {
        Errors = new Dictionary<string, string[]>
        {
            { key, errors }
        };
    }

    public ErrorMessages(string[] keys, params string[] errors)
    {
        Errors = keys.ToDictionary(x => x, x => errors);
    }
}
