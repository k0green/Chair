namespace Chair.BLL.Dto.ProductFile;

public class FileViewDto
{
    public Guid Id { get; set; }
    public DateTime CreateDate { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }
}

public class FileFullViewDto<T> : FileViewDto
{
    public T Entity { get; set; }

}