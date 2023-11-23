namespace Chair.DAL.Extension.Models;

public class KeywordSearchModel
{
    public string FieldName { get; set; }
    public string SearchText { get; set; }

    internal string[] Words => SearchText.Trim().Split(new char[] { ',', '.', ';', ' ' }).Where(w => w.Length > 0).ToArray();
    internal string LongestWord => Words.OrderByDescending(w => w.Length).FirstOrDefault();
    internal bool IsValid => !String.IsNullOrEmpty(FieldName) && Words.Length > 0;
}