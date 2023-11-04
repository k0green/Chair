using Chair.BLL.Dto.ExecutorService;

namespace Chair.BLL.BusinessLogic.ExecutorProfile
{
    public interface IExecutorProfileBusinessLogic
    {
        Task<List<ExecutorProfileDto>> GetAllProfilesByServiceTypeId(Guid serviceTypeId);
        Task<ExecutorProfileDto> GetExecutorProfileByUserId();
        Task<ExecutorProfileDto> GetExecutorProfileById(Guid id);

        Task<Guid> AddAsync(AddExecutorProfileDto dto);

        Task UpdateAsync(UpdateExecutorProfileDto dto);

        Task RemoveAsync(Guid id);
    }
}
