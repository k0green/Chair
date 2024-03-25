using Chair.BLL.CQRS.Minio;
using Chair.DAL.Data;
using FluentValidation;

namespace Chair.BLL.Validation.Minio;

public class UploadMinioFileCommandValidator : AbstractValidator<UploadMinioFileCommand>
{
    private readonly ApplicationDbContext _context;
    
    public UploadMinioFileCommandValidator(ApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.AddMinioFileDto).NotNull().WithMessage("{PropertyName} can't be null");

        RuleFor(x => x.AddMinioFileDto.FileData).MustAsync(async (file, token) =>
        {
            return file.Length > 0;
        }).WithMessage("File is empty");
    }
}
