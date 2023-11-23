namespace Chair.DAL.Extension.Models
{
    public class FilterModel
    {
        public int? Skip { get; set; }
        public int? Take { get; set; }
        public IEnumerable<Sort> Sort { get; set; }
        public Filter? Filter { get; set; } = new Filter();
    }
}