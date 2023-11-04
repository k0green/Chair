using Chair.BLL.Dto.Base;

namespace Chair.BLL.Dto.ExecutorService
{
    public class GroupExecutorServiceDto : BaseDto
    {
        public string ServiceTypeName { get; set; }
        public List<ExecutorServiceDto> Services { get; set; }
    }
}
