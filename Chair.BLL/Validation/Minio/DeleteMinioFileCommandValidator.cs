using Chair.BLL.CQRS.Minio;
using Chair.DAL.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Chair.BLL.Validation.Minio;

public class DeleteMinioFileCommandValidator : AbstractValidator<DeleteMinioFileCommand>
{
    private readonly ApplicationDbContext _context;
    
    public DeleteMinioFileCommandValidator(ApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Id).MustAsync(async (id, token) =>
        {
            var contractMonthlyService = await _context.MinioFiles.FirstOrDefaultAsync(x => x.Id == id);

            return contractMonthlyService != null;
        }).WithMessage("File with Id: {PropertyValue} doesnt exist");
    }
}
