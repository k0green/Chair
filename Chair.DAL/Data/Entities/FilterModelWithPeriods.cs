using Chair.DAL.Extension.Models;

namespace Chair.DAL.Data.Entities;

public class FilterModelWithPeriods
{
    public FilterModel Filter { get; set; }
    public List<DateTime>? Dates { get; set; }
    public List<TimePeriod>? Times { get; set; }
}

public class TimePeriod
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}