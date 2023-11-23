using Chair.BLL.BusinessLogic.ExecutorService;
using Chair.BLL.BusinessLogic.Message;
using Chair.BLL.CQRS.Message;
using Chair.BLL.CQRS.ServiceType;
using MediatR;

namespace Chair.BLL.MediatR.Message
{
    public class RemoveMessageHandler : IRequestHandler<RemoveMessageQuery, Unit>
    {
        private readonly IMessageBusinessLogic _messageBusinessLogic;

        public RemoveMessageHandler(IMessageBusinessLogic messageBusinessLogic)
        {
            _messageBusinessLogic = messageBusinessLogic;
        }

        public async Task<Unit> Handle(RemoveMessageQuery request, CancellationToken cancellationToken)
        {
            await _messageBusinessLogic.RemoveAsync(request.Id);

            return Unit.Value;
        }
    }
}
