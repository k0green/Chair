using Chair.BLL.BusinessLogic.Message;
using Chair.BLL.CQRS.Message;
using MediatR;

namespace Chair.BLL.MediatR.Message
{
    public class EditMessageTextHandler : IRequestHandler<EditMessageTextQuery, Unit>
    {
        private readonly IMessageBusinessLogic _messageBusinessLogic;

        public EditMessageTextHandler(IMessageBusinessLogic messageBusinessLogic)
        {
            _messageBusinessLogic = messageBusinessLogic;
        }

        public async Task<Unit> Handle(EditMessageTextQuery request, CancellationToken cancellationToken)
        {
            await _messageBusinessLogic.EditText(request.LookupDto);

            return Unit.Value;
        }
    }
}
