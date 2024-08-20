using MediatR;
using Chair.BLL.Dto.ExecutorPromotion;
using Chair.DAL.Extension.Models;

namespace Chair.BLL.CQRS.ExecutorPromotion
{
    public class GetAllPromotionsQuery : IRequest<(List<ExecutorPromotionDto>, int)>
    {
        public FilterModel Filter { get; set; }
    }
}
