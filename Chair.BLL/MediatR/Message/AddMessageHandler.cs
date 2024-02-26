using Chair.BLL.BusinessLogic.ExecutorService;
using Chair.BLL.BusinessLogic.Message;
using Chair.BLL.CQRS.Message;
using Chair.BLL.CQRS.ServiceType;
using Chair.BLL.Dto.Message;
using MediatR;

namespace Chair.BLL.MediatR.Message
{
    public class AddMessageHandler : IRequestHandler<AddMessageQuery, MessageDto>
    {
        private readonly IMessageBusinessLogic _messageBusinessLogic;

        public AddMessageHandler(IMessageBusinessLogic messageBusinessLogic)
        {
            _messageBusinessLogic = messageBusinessLogic;
        }

        public async Task<MessageDto> Handle(AddMessageQuery request, CancellationToken cancellationToken)
        {
            var result = await _messageBusinessLogic.AddAsync(request.AddMessageDto);

            return result;
        }
    }
}
