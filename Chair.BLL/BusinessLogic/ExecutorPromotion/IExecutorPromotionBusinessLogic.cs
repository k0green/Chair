using Chair.BLL.Dto.ExecutorPromotion;
using Chair.DAL.Extension.Models;
namespace Chair.BLL.BusinessLogic.ExecutorPromotion
{
    public interface IExecutorPromotionBusinessLogic
    {
        Task<List<ExecutorPromotionDto>> GetAllPromotionsByExecutorId(Guid executorId);
        Task<List<ExecutorPromotionDto>> GetAllPromotions(FilterModel filter);
        Task<ExecutorPromotionDto> GetByIdPromotion(Guid id);
        Task<Guid> AddAsync(AddExecutorPromotionDto dto);

        Task UpdateAsync(UpdateExecutorPromotionDto dto);

        Task RemoveAsync(Guid id);
    }
}
