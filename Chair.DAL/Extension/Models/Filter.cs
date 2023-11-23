using Chair.DAL.Enums;

namespace Chair.DAL.Extension.Models
{
    public class Filter
    {
        public string Field { get; set; }
        public string Operator { get; set; }
        public object Value { get; set; }
        public string Logic { get; set; }
        public MatchCaseEnum MatchCaseSettings { get; set; }
        public IEnumerable<Filter> Filters { get; set; }
    }
}