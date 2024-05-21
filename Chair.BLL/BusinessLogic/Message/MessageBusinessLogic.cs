using AutoMapper;
using Chair.BLL.BusinessLogic.Account;
using Chair.BLL.Commons;
using Chair.BLL.Dto.Base;
using Chair.BLL.Dto.Message;
using Chair.DAL.Repositories.Base;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ChatEntity = Chair.DAL.Data.Entities.Chat;

namespace Chair.BLL.BusinessLogic.Message
{
    public class MessageBusinessLogic : IMessageBusinessLogic
    {
        private readonly IBaseWithManyRepository<DAL.Data.Entities.Message> _messageRepository;
        private readonly IHubContext<MessageHub> _hubContext;
        private readonly IBaseWithManyRepository<ChatEntity> _chatRepository;
        private readonly UserInfo _userInfo;
        private readonly IMapper _mapper;

        public MessageBusinessLogic(IBaseWithManyRepository<DAL.Data.Entities.Message> messageRepository,
            IHubContext<MessageHub> hubContext,
            IBaseWithManyRepository<ChatEntity> chatRepository,
            UserInfo userInfo,
            IMapper mapper)
        {
            _messageRepository = messageRepository;
            _chatRepository = chatRepository;
            _hubContext = hubContext;
            _userInfo = userInfo;
            _mapper = mapper;
        }

        public async Task<MessageDto> AddAsync(AddMessageDto dto)
        {
            var chat = await _chatRepository.GetByIdAsync(dto.ChatId);
            if (chat == null)
            {
                await _chatRepository.AddAsync(new DAL.Data.Entities.Chat()
                {
                    Id = dto.ChatId,
                    IsDeleted = false,
                });
                await _chatRepository.SaveChangesAsync();
            }
            var entity = _mapper.Map<DAL.Data.Entities.Message>(dto);
            entity.CreatedDate = DateTime.Now;
            await _messageRepository.AddAsync(entity);
            await _messageRepository.SaveChangesAsync();
            var result = _mapper.Map<MessageDto>(entity);
            await _hubContext.Clients.User(dto.RecipientId).SendAsync("ReceiveMessage", result);
            return result;
        }

        public async Task<Guid> EditText(LookupDto dto)
        {
            var entity = await _messageRepository.GetByIdAsync(dto.Id);
            entity.Text = dto.Name;
            await _messageRepository.UpdateAsync(entity);
            await _messageRepository.SaveChangesAsync();
            await _hubContext.Clients.User(entity.RecipientId).SendAsync("ReceiveMessage", _mapper.Map<MessageDto>(entity));
            return entity.Id;
        }

        public async Task MarkAsRead(Guid chatId)
        {
            var recipientId = await _userInfo.GetUserIdFromToken();
            var messages = await _messageRepository
                .GetAllByPredicateAsQueryable(x => x.ChatId == chatId)
                .Where(x => x.RecipientId == recipientId)
                .Where(x => !x.IsDeleted)
                .Where(x => !x.IsRead)
                .ToListAsync();
            if (messages.Any())
            {
                messages.ForEach(x => x.IsRead = true);
                await _messageRepository.UpdateManyAsync(messages);
                await _messageRepository.SaveChangesAsync();
                await _hubContext.Clients.Users(messages.First().SenderId, recipientId).SendAsync("ReceiveAllMessages", _mapper.Map<List<MessageDto>>(messages));   
            }
        }

        public async Task RemoveAsync(Guid id)
        {
            var entity = await _messageRepository.GetByIdAsync(id);
            entity.IsDeleted = true;
            await _messageRepository.UpdateAsync(entity);
            await _messageRepository.SaveChangesAsync();
            await _hubContext.Clients.User(entity.RecipientId).SendAsync("ReceiveMessage", _mapper.Map<MessageDto>(entity));
        }
    }
}
