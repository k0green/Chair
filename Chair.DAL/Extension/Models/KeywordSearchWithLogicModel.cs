namespace Chair.DAL.Extension.Models;

public class KeywordSearchWithLogicModel
{
        public string Logic { get; set; }
        public IEnumerable<KeywordSearchModel> Search { get; set; }
}