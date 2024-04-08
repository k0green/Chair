using AutoMapper;
using Chair.BLL.BusinessLogic.Account;
using Chair.BLL.BusinessLogic.ExecutorService;
using Chair.BLL.Dto.ExecutorService;
using Chair.DAL.Data.Entities;
using Chair.DAL.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using ExecutorProfileEntity = Chair.DAL.Data.Entities.ExecutorProfile;
using ExecutorServiceEntity = Chair.DAL.Data.Entities.ExecutorService;

namespace Chair.BLL.BusinessLogic.ExecutorProfile
{
    public class ExecutorProfileBusinessLogic : IExecutorProfileBusinessLogic
    {
        private readonly IBaseRepository<ExecutorProfileEntity> _executorProfileRepository;
        private readonly IBaseRepository<ExecutorServiceEntity> _executorServiceRepository;
        private readonly IExecutorServiceBusinessLogic _executorServiceBusiness;
        private readonly IBaseWithManyRepository<Contact> _contactRepository;
        private readonly UserInfo _userInfo;
        private readonly IMapper _mapper;

        public ExecutorProfileBusinessLogic(IBaseRepository<ExecutorProfileEntity> executorProfileRepository,
            IBaseRepository<ExecutorServiceEntity> executorServiceRepository,
            IExecutorServiceBusinessLogic executorServiceBussinesLogic,
            IBaseWithManyRepository<Contact> contactRepository,
            UserInfo userInfo,
            IMapper mapper)
        {
            _executorServiceRepository = executorServiceRepository;
            _executorProfileRepository = executorProfileRepository;
            _executorServiceBusiness = executorServiceBussinesLogic;
            _contactRepository = contactRepository;
            _userInfo = userInfo;
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
            executorProfileDto.Services = await _executorServiceBusiness.GetAllServicesByExecutorId(id);
            return executorProfileDto;
        }

        public async Task<ExecutorProfileDto> GetExecutorProfileByUserId()
        {
            var userId = await _userInfo.GetUserIdFromToken();
            var executorProfile = await _executorProfileRepository
                .GetAllByPredicateAsQueryable(x => x.UserId == userId)
                .Include(x => x.User)
                .Include(x=>x.Contacts)
                .Include(x=>x.Image)
                .FirstAsync();
            var executorProfileDto = _mapper.Map<ExecutorProfileDto>(executorProfile);
            executorProfileDto.Services = await _executorServiceBusiness.GetAllServicesByExecutorId(executorProfile.Id);
            return executorProfileDto;
        }

        public async Task<Guid> AddAsync(AddExecutorProfileDto dto)
        {
            var entity = _mapper.Map<DAL.Data.Entities.ExecutorProfile>(dto);
            entity.Id = Guid.NewGuid();
            entity.Description = "";
            var contactsEntity = _mapper.Map<List<Contact>>(dto.Contacts);
            contactsEntity.ForEach(x=>
            {
                x.ExecutorProfileId = entity.Id;
                x.Id = Guid.NewGuid();
            });
            await _executorProfileRepository.AddAsync(entity);
            await _executorProfileRepository.SaveChangesAsync();

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
        }

        public async Task RemoveAsync(Guid id)
        {
            await _executorProfileRepository.RemoveByIdAsync(id);
            await _executorProfileRepository.SaveChangesAsync();
        }
    }
}
