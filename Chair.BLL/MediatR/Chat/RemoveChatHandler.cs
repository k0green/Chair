using Chair.BLL.BusinessLogic.Chat;
using Chair.BLL.CQRS.Chat;
using MediatR;

namespace Chair.BLL.MediatR.Chat
{
    public class RemoveExecutorProfileHandler : IRequestHandler<RemoveChatQuery, Unit>
    {
        private readonly IChatBusinessLogic _chatBusinessLogic;

        public RemoveExecutorProfileHandler(IChatBusinessLogic chatBusinessLogic)
        {
            _chatBusinessLogic = chatBusinessLogic;
        }

        public async Task<Unit> Handle(RemoveChatQuery request, CancellationToken cancellationToken)
        {
            await _chatBusinessLogic.RemoveAsync(request.Id);

            return Unit.Value;
        }
    }
}
