using Chair.BLL.Dto.Base;
using Chair.DAL.Enums;

namespace Chair.BLL.Dto.Contacts
{
    public class UpdateContactsDto : BaseDto
    {
        public string Name { get; set; }
        public ContactsType Type { get; set; }
        public Guid ExecutorProfileId { get; set; }
    }
}
