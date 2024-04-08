using AutoMapper;
using Chair.BLL.BusinessLogic.Account;
using Microsoft.EntityFrameworkCore;
using Chair.BLL.Dto.Chat;
using Chair.BLL.Dto.Message;
using Chair.DAL.Repositories.Base;
using ChatEntity = Chair.DAL.Data.Entities.Chat;

namespace Chair.BLL.BusinessLogic.Chat
{
    public class ChatBusinessLogic : IChatBusinessLogic
    {
        private readonly IBaseWithManyRepository<ChatEntity> _chatRepository;
        private readonly IBaseRepository<DAL.Data.Entities.ExecutorProfile> _executorProfileRepository;
        private readonly IBaseWithManyRepository<DAL.Data.Entities.Message> _messageRepository;
        private readonly IMapper _mapper;
        private readonly UserInfo _userInfo;

        public ChatBusinessLogic(IBaseWithManyRepository<ChatEntity> chatRepository,
            IBaseRepository<DAL.Data.Entities.ExecutorProfile> executorProfileRepository,
            IBaseWithManyRepository<DAL.Data.Entities.Message> messageRepository,
            UserInfo userInfo,
            IMapper mapper)
        {
            _chatRepository = chatRepository;
            _executorProfileRepository = executorProfileRepository;
            _messageRepository = messageRepository;
            _mapper = mapper;
            _userInfo = userInfo;
        }

        public async Task<List<ChatDto>> GetAllChatForCurrentUser()
        {
            var userId = await _userInfo.GetUserIdFromToken();

            var profiles = await _executorProfileRepository.GetAllByPredicateAsQueryable().ToListAsync();

            var chatIds = await _messageRepository
                .GetAllByPredicateAsQueryable(x => x.SenderId == userId || x.RecipientId == userId)
                .Select(x => new
                {
                    ChatId = x.ChatId,
                    RecipientId = x.RecipientId == userId ? x.SenderId : x.RecipientId
                })
                .Distinct()
                .ToListAsync();

            var chats = await _chatRepository
                .GetAllByPredicateAsQueryable(x => chatIds.Select(c => c.ChatId).Contains(x.Id) && !x.IsDeleted)
                .Include(x => x.Messages)
                .ToListAsync();

            var chatDtos = chats
                .Select(x => new ChatDto
                {
                    Id = x.Id,
                    RecipientName = profiles.First(p => p.UserId == chatIds.First(c => c.ChatId == x.Id).RecipientId).Name,
                    RecipientProfileId = profiles.First(p => p.UserId == chatIds.First(c => c.ChatId == x.Id).RecipientId).Id,
                    RecipientProfileImg = profiles.First(p => p.UserId == chatIds.First(c => c.ChatId == x.Id).RecipientId)?.Image?.Url,
                    UnreadMessagesAmount = x.Messages.Where(c => c.ChatId == x.Id && c.RecipientId == userId && c is { IsDeleted: false, IsRead: false }).ToList().Count,
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
                    RecipientProfileImg = profile.Image.Url,
                    Messages = new List<MessageDto>(),
                };
            }

            var messages = await _messageRepository
                .GetAllByPredicateAsQueryable(x => x.ChatId == message.ChatId)
                .Where(x => !x.IsDeleted)
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
                    RecipientProfileImg = profile.Image != null ? profile.Image.Url : "",
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
