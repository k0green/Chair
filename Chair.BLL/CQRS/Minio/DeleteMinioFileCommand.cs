using MediatR;

namespace Chair.BLL.CQRS.Minio;

public class DeleteMinioFileCommand : IRequest<Unit>
{
	public Guid Id { get; set; }
}
