using Chair.BLL.Dto.Base;

namespace Chair.BLL.Dto.ExecutorService
{
    public class ExecutorServiceDto : BaseDto
    {
        public Guid ServiceTypeId { get; set; }
        public string ServiceTypeName { get; set; }
        public string ExecutorId { get; set; }
        public string Description { get; set; }
    }
}
