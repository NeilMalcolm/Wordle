namespace Wordle.App;

public static class ResourceDictionaryExtensions
{
    public static Style GetStyleFromResourceDictionary(this ResourceDictionary dictionary, string resourceName)
    {
        if (dictionary.TryGetValue(resourceName, out object value) && value is Style style)
        {
            return style;
        }

        return null;
    }
}
