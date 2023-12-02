using Chair.DAL.Data.Entities;
using Chair.DAL.Extension.Models;
using opr_lib;

namespace Chair.BLL.Dto.ExecutorService;

public class GetOptimizeServiceDto
{
    public FilterModelWithPeriods FilterModel { get; set; }
    public Guid ServiceTypeId { get; set; }
    public List<Condition> Conditions { get; set; }
}