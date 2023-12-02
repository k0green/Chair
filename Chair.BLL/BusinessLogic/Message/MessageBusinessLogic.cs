﻿using AutoMapper;
using Chair.BLL.BusinessLogic.Account;
using Chair.BLL.Dto.Base;
using Chair.BLL.Dto.Message;
using Chair.DAL.Repositories.Chat;
using Chair.DAL.Repositories.Message;
using Microsoft.EntityFrameworkCore;

namespace Chair.BLL.BusinessLogic.Message
{
    public class MessageBusinessLogic : IMessageBusinessLogic
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IChatRepository _chatRepository;
        private readonly UserInfo _userInfo;
        private readonly IMapper _mapper;

        public MessageBusinessLogic(IMessageRepository messageRepository,
            IChatRepository chatRepository,
            UserInfo userInfo,
            IMapper mapper)
        {
            _messageRepository = messageRepository;
            _chatRepository = chatRepository;
            _userInfo = userInfo;
            _mapper = mapper;
        }

        public async Task<Guid> AddAsync(AddMessageDto dto)
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
            entity.Id = Guid.NewGuid();
            entity.CreatedDate = DateTime.Now;
            await _messageRepository.AddAsync(entity);
            await _messageRepository.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<Guid> EditText(LookupDto dto)
        {
            var entity = await _messageRepository.GetByIdAsync(dto.Id);
            entity.Text = dto.Name;
            await _messageRepository.UpdateAsync(entity);
            await _messageRepository.SaveChangesAsync();
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
            messages.ForEach(x => x.IsRead = true);
            await _messageRepository.UpdateManyAsync(messages);
            await _messageRepository.SaveChangesAsync();
        }

        public async Task RemoveAsync(Guid id)
        {
            await _messageRepository.RemoveByIdAsync(id);
            await _messageRepository.SaveChangesAsync();
        }
    }
}