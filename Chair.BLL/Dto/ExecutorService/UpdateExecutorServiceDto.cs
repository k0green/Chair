using Chair.BLL.Dto.Base;

namespace Chair.BLL.Dto.ExecutorService
{
    public class UpdateExecutorServiceDto : BaseDto
    {
        public Guid ServiceTypeId { get; set; }
        public Guid ExecutorId { get; set; }
        public string Description { get; set; }
        public Place Place { get; set; }
        public DateTime Duration { get; set; }
        public decimal Price { get; set; }
        public List<Guid> PhotoIds { get; set; }
        public List<Guid> RemovePhotoIds { get; set; }
    }
}
