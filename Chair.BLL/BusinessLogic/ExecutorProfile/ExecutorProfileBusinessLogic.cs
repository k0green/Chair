﻿using AutoMapper;
using Chair.BLL.Dto.ExecutorService;
using Chair.DAL.Data.Entities;
using Chair.DAL.Repositories.Contact;
using Chair.DAL.Repositories.ExecutorProfile;
using Chair.DAL.Repositories.ExecutorService;
using Microsoft.EntityFrameworkCore;

namespace Chair.BLL.BusinessLogic.ExecutorProfile
{
    public class ExecutorProfileBusinessLogic : IExecutorProfileBusinessLogic
    {
        private readonly IExecutorProfileRepository _executorProfileRepository;
        private readonly IExecutorServiceRepository _executorServiceRepository;
        private readonly IContactRepository _contactRepository;
        private readonly IMapper _mapper;

        public ExecutorProfileBusinessLogic(IExecutorProfileRepository executorProfileRepository,
            IExecutorServiceRepository executorServiceRepository,
            IContactRepository contactRepository,
            IMapper mapper)
        {
            _executorProfileRepository = executorProfileRepository;
            _executorServiceRepository = executorServiceRepository;
            _contactRepository = contactRepository;
            _mapper = mapper;
        }

        public async Task<List<ExecutorProfileDto>> GetAllProfilesByServiceTypeId(Guid serviceTypeId)
        {
            var executorIds = await _executorServiceRepository
                .GetAllByPredicateAsQueryable(x => x.ServiceTypeId == serviceTypeId).Select(x => x.ExecutorId).ToListAsync();
            var executorProfiles = await _executorProfileRepository
                .GetAllByPredicateAsQueryable(x => executorIds.Contains(x.Id))
                .Include(x=>x.Contacts).ToListAsync();
            var executorProfilesDtos = _mapper.Map<List<ExecutorProfileDto>>(executorProfiles);
            return executorProfilesDtos;
        }

        public async Task<ExecutorProfileDto> GetExecutorProfileById(Guid id)
        {
            var executorProfile = await _executorProfileRepository
                .GetAllByPredicateAsQueryable(x => x.Id == id)
                .Include(x => x.User)
                .Include(x=>x.Contacts)
                .FirstOrDefaultAsync();
            var executorProfileDto = _mapper.Map<ExecutorProfileDto>(executorProfile);
            return executorProfileDto;
        }

        public async Task<Guid> AddAsync(AddExecutorProfileDto dto)
        {
            var entity = _mapper.Map<DAL.Data.Entities.ExecutorProfile>(dto);
            entity.Id = Guid.NewGuid();
            var contactsEntity = _mapper.Map<List<Contact>>(dto.Contacts);
            contactsEntity.ForEach(x=>
            {
                x.ExecutorProfileId = entity.Id;
                x.Id = Guid.NewGuid();
            });
            await _executorProfileRepository.AddAsync(entity);
            await _executorProfileRepository.SaveChangesAsync();

            //await _contactRepository.AddManyAsync(contactsEntity);
            //await _contactRepository.SaveChangesAsync();

            return entity.Id;
        }

        public async Task UpdateAsync(UpdateExecutorProfileDto dto)
        {
            var entity = _mapper.Map<DAL.Data.Entities.ExecutorProfile>(dto);
            var contactEntityIds = await _contactRepository
                .GetAllByPredicateAsQueryable(x => x.ExecutorProfileId == dto.Id)
                .Select(x => x.Id)
                .ToListAsync();
            await _contactRepository.RemoveManyByIdsAsync(contactEntityIds);
            var contactsEntity = _mapper.Map<List<Contact>>(dto.Contacts);
            contactsEntity.ForEach(x =>
            {
                x.ExecutorProfileId = entity.Id;
                x.Id = Guid.NewGuid();
            });

            await _executorProfileRepository.UpdateAsync(entity);
            await _executorProfileRepository.SaveChangesAsync();

            //await _contactRepository.AddManyAsync(contactsEntity);
            //await _contactRepository.SaveChangesAsync();
        }

        public async Task RemoveAsync(Guid id)
        {
            await _executorProfileRepository.RemoveByIdAsync(id);
            await _executorProfileRepository.SaveChangesAsync();
        }
    }
}