using Chair.BLL.BusinessLogic.Chat;
using Chair.BLL.CQRS.Chat;
using Chair.BLL.CQRS.ServiceType;
using Chair.BLL.Dto.Chat;
using MediatR;

namespace Chair.BLL.MediatR.Chat
{
    public class GetChatForProfileHandler : IRequestHandler<GetChatForProfileQuery, ChatDto>
    {
        private readonly IChatBusinessLogic _chatBusinessLogic;

        public GetChatForProfileHandler(IChatBusinessLogic chatBusinessLogic)
        {
            _chatBusinessLogic = chatBusinessLogic;
        }

        public async Task<ChatDto> Handle(GetChatForProfileQuery request, CancellationToken cancellationToken)
        {
            var result = await _chatBusinessLogic.GetChatForProfile(request.ProfileId);

            return result;
        }
    }
}
