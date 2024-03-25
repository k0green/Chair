using Chair.BLL.CQRS.Minio;
using Chair.DAL.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Chair.BLL.Validation.Minio;

public class DownloadMinioFileQueryValidator : AbstractValidator<DownloadMinioFileQuery>
{
    private readonly ApplicationDbContext _context;
    
    public DownloadMinioFileQueryValidator(ApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Id).MustAsync(async (id, token) =>
        {
            var contract = await _context.MinioFiles.FirstOrDefaultAsync(x => x.Id == id);

            return contract != null;
        }).WithMessage("File with Id: {PropertyValue} doesnt exist");
    }
}
