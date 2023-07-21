using Chair.BLL.Dto.Base;

namespace Chair.BLL.Dto.ExecutorService
{
    public class UpdateExecutorServiceDto : BaseDto
    {
        public Guid ServiceTypeId { get; set; }
        public string ExecutorId { get; set; }
        public string Description { get; set; }
    }
}
