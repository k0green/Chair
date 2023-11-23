using Chair.DAL.Extension.Models;
using opr_lib;

namespace Chair.BLL.Dto.ExecutorService;

public class GetOptimizeServiceDto
{
    public FilterModel FilterModel { get; set; }
    public Guid ServiceTypeId { get; set; }
    public List<Condition> Conditions { get; set; }
}