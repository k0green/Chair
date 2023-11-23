using Chair.BLL.BusinessLogic.Chat;
using Chair.BLL.CQRS.Chat;
using Chair.BLL.Dto.Chat;
using MediatR;

namespace Chair.BLL.MediatR.Chat
{
    public class GetAllChatForCurrentUserHandler : IRequestHandler<GetAllChatForCurrentUserQuery, List<ChatDto>>
    {
        private readonly IChatBusinessLogic _chatBusinessLogic;

        public GetAllChatForCurrentUserHandler(IChatBusinessLogic chatBusinessLogic)
        {
            _chatBusinessLogic = chatBusinessLogic;
        }

        public async Task<List<ChatDto>> Handle(GetAllChatForCurrentUserQuery request, CancellationToken cancellationToken)
        {
            var result = await _chatBusinessLogic.GetAllChatForCurrentUser();

            return result;
        }
    }
}
