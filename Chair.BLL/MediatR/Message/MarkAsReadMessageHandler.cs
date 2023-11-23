using Chair.BLL.BusinessLogic.ExecutorService;
using Chair.BLL.BusinessLogic.Message;
using Chair.BLL.CQRS.ExecutorService;
using Chair.BLL.Dto.ExecutorService;
using MediatR;

namespace Chair.BLL.MediatR.Message
{
    public class MarkAsReadMessageHandler : IRequestHandler<MarkAsReadMessageQuery, Unit>
    {
        private readonly IMessageBusinessLogic _messageBusinessLogic;

        public MarkAsReadMessageHandler(IMessageBusinessLogic messageBusinessLogic)
        {
            _messageBusinessLogic = messageBusinessLogic;
        }

        public async Task<Unit> Handle(MarkAsReadMessageQuery request, CancellationToken cancellationToken)
        {
            await _messageBusinessLogic.MarkAsRead(request.ChatId);

            return Unit.Value;
        }
    }
}
