namespace Chair.DAL.Extension.Models;

public class SearchByKeywordsRegexBuilder
{
    public static string Build(string[] words)
    {
        return String.Join("", words.Select(word => "(?=.*" + word + ")")) + ".*";
    }
}