using Chair.BLL.Dto.Message;
using MediatR;

namespace Chair.BLL.CQRS.Message
{
    public class AddMessageQuery : IRequest<Guid>
    {
        public AddMessageDto AddMessageDto { get; set; }
    }
}
