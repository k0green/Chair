using Chair.BLL.Dto.Base;
using MediatR;

namespace Chair.BLL.CQRS.Message
{
    public class EditMessageTextQuery : IRequest<Unit>
    {
        public LookupDto LookupDto { get; set; }
    }
}
