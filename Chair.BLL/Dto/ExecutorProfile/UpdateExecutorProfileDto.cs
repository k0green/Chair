using Chair.BLL.Dto.Base;

namespace Chair.BLL.Dto.ExecutorService
{
    public class UpdateExecutorProfileDto : BaseDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
    }
}
