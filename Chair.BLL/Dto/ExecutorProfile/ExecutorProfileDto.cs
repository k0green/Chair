using Chair.BLL.Dto.Base;
using Chair.BLL.Dto.Contacts;

namespace Chair.BLL.Dto.ExecutorService
{
    public class ExecutorProfileDto : BaseDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string ImageUrl { get; set; }
        public List<ContactsDto> Contacts { get; set; }
    }
}
