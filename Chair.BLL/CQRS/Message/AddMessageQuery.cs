using Chair.BLL.Dto.Message;
using MediatR;

namespace Chair.BLL.CQRS.Message
{
    public class AddMessageQuery : IRequest<MessageDto>
    {
        public AddMessageDto AddMessageDto { get; set; }
    }
}
