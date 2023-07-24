using Chair.BLL.Dto.Base;

namespace Chair.BLL.Dto.ExecutorService
{
    public class ExecutorServiceDto : BaseDto
    {
        public Guid ServiceTypeId { get; set; }
        public string ServiceTypeName { get; set; }
        public Guid ExecutorId { get; set; }
        public string Description { get; set; }
    }
}
