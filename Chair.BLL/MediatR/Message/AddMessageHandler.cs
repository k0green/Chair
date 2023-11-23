using Chair.BLL.BusinessLogic.ExecutorService;
using Chair.BLL.BusinessLogic.Message;
using Chair.BLL.CQRS.Message;
using Chair.BLL.CQRS.ServiceType;
using MediatR;

namespace Chair.BLL.MediatR.Message
{
    public class AddMessageHandler : IRequestHandler<AddMessageQuery, Guid>
    {
        private readonly IMessageBusinessLogic _messageBusinessLogic;

        public AddMessageHandler(IMessageBusinessLogic messageBusinessLogic)
        {
            _messageBusinessLogic = messageBusinessLogic;
        }

        public async Task<Guid> Handle(AddMessageQuery request, CancellationToken cancellationToken)
        {
            var result = await _messageBusinessLogic.AddAsync(request.AddMessageDto);

            return result;
        }
    }
}
