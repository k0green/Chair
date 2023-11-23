using AutoMapper;
using Chair.BLL.BusinessLogic.Account;
using Chair.DAL.Repositories.Contact;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using Chair.BLL.Commons;
using Chair.BLL.Dto.Chat;
using Chair.BLL.Dto.Message;
using Chair.DAL.Repositories.Chat;
using Chair.DAL.Repositories.ExecutorProfile;
using Chair.DAL.Repositories.Message;

namespace Chair.BLL.BusinessLogic.Chat
{
    public class ChatBusinessLogic : IChatBusinessLogic
    {
        private readonly IChatRepository _chatRepository;
        private readonly IExecutorProfileRepository _executorProfileRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IContactRepository _contactRepository;
        private readonly IMapper _mapper;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly UserInfo _userInfo;

        public ChatBusinessLogic(IChatRepository chatRepository,
            IExecutorProfileRepository executorProfileRepository,
            IMessageRepository messageRepository,
            IContactRepository contactRepository,
            IHubContext<NotificationHub> hubContext,
            UserInfo userInfo,
            IMapper mapper)
        {
            _chatRepository = chatRepository;
            _executorProfileRepository = executorProfileRepository;
            _messageRepository = messageRepository;
            _contactRepository = contactRepository;
            _hubContext = hubContext;
            _mapper = mapper;
            _userInfo = userInfo;
        }

        public async Task<List<ChatDto>> GetAllChatForCurrentUser()
        {
            var userId = await _userInfo.GetUserIdFromToken();

            // Fetch profiles first
            var profiles = await _executorProfileRepository.GetAllByPredicateAsQueryable().ToListAsync();

            // Fetch chat IDs with recipients
            var chatIds = await _messageRepository
                .GetAllByPredicateAsQueryable(x => x.SenderId == userId || x.RecipientId == userId)
                .Select(x => new
                {
                    ChatId = x.ChatId,
                    RecipientId = x.RecipientId == userId ? x.SenderId : x.RecipientId
                })
                .Distinct()
                .ToListAsync();

            // Fetch all chats
            var chats = await _chatRepository
                .GetAllByPredicateAsQueryable(x => chatIds.Select(c => c.ChatId).Contains(x.Id) && !x.IsDeleted)
                .Include(x => x.Messages)
                .ToListAsync();

            // Map data in memory to avoid translation issues
            var chatDtos = chats
                .Select(x => new ChatDto
                {
                    Id = x.Id,
                    RecipientName = profiles.First(p => p.UserId == chatIds.First(c => c.ChatId == x.Id).RecipientId).Name,
                    RecipientProfileId = profiles.First(p => p.UserId == chatIds.First(c => c.ChatId == x.Id).RecipientId).Id,
                    RecipientProfileImg = profiles.First(p => p.UserId == chatIds.First(c => c.ChatId == x.Id).RecipientId).ImageURL,
                    UnreadMessagesAmount = x.Messages.Where(c => c.ChatId == x.Id && !c.IsRead).ToList().Count,
                    Messages = _mapper.Map<List<MessageDto>>(x.Messages
                        .Where(c => c.ChatId == x.Id)
                        .OrderByDescending(c => c.CreatedDate)
                        .Take(1))
                })
                .ToList();

            return chatDtos;
        }


        public async Task<ChatDto> GetChatForProfile(Guid profileId)
        {
            var userId = await _userInfo.GetUserIdFromToken();
            var profile = _executorProfileRepository.GetAllByPredicateAsQueryable(e => e.Id == profileId).First();

            var message = await _messageRepository
                .GetAllByPredicateAsQueryable(x => x.RecipientId == profile.UserId && x.SenderId == userId 
                                                   || x.RecipientId == userId && x.SenderId == profile.UserId
                                                   || x.ChatId == profileId)
                .FirstOrDefaultAsync();

            if (message == null)
            {
                return new ChatDto
                {
                    Id = Guid.NewGuid(),
                    SenderId = userId,
                    RecipientId = profile.UserId,
                    RecipientName = profile.Name,
                    RecipientProfileId = profile.Id,
                    RecipientProfileImg = profile.ImageURL,
                    Messages = new List<MessageDto>(),
                };
            }

            var messages = await _messageRepository
                .GetAllByPredicateAsQueryable(x => x.ChatId == message.ChatId)
                .OrderBy(x => x.CreatedDate)
                .ToListAsync();
            
            var chats = await _chatRepository
                .GetAllByPredicateAsQueryable(x => x.Id == message.ChatId)
                .Select(x => new ChatDto
                {
                    Id = x.Id,
                    SenderId = userId,
                    RecipientId = profile.UserId,
                    RecipientName = profile.Name,
                    RecipientProfileId = profile.Id,
                    RecipientProfileImg = profile.ImageURL,
                    Messages = _mapper.Map<List<MessageDto>>(messages),
                }).FirstAsync();
            
            return chats;
        }

        public async Task<DAL.Data.Entities.Chat> AddAsync(DAL.Data.Entities.Chat entity)
        {
            entity.Id = Guid.NewGuid();
            await _chatRepository.AddAsync(entity);
            await _chatRepository.SaveChangesAsync();

            return entity;
        }

        public async Task RemoveAsync(Guid id)
        {
            var chat = _chatRepository.GetAllByPredicateAsQueryable(x => x.Id == id).First();
            chat.IsDeleted = true;
            await _chatRepository.UpdateAsync(chat);
            await _chatRepository.SaveChangesAsync();
        }
    }
}
