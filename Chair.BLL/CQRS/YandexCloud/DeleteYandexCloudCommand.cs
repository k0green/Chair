using MediatR;

namespace Chair.BLL.CQRS.YandexCloud;

public class DeleteYandexCloudCommand : IRequest<Unit>
{
	public Guid Id { get; set; }
}
