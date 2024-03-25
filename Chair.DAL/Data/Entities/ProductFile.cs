using Microsoft.EntityFrameworkCore;

namespace Chair.DAL.Data.Entities;

[PrimaryKey(nameof(ProductId), nameof(MinioFileId))]
public class ProductFile<T> : ProductFile where T : class
{
    public T Product { get; set; }
}

public abstract class ProductFile : BaseEntity
{
    public Guid ProductId { get; set; }
    public Guid MinioFileId { get; set; }
    public MinioFile MinioFile { get; set; }
}