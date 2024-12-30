using Chair.BLL.Dto.Base;

namespace Chair.BLL.Dto.ServiceType
{
    public class ServiceTypeDto : BaseDto
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public Guid? ParentId { get; set; }
        public string ParentName { get; set; }
    }
}
