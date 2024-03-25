namespace Chair.BLL.Dto.Minio;

public class MinioFileFullDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }
    public DateTime CreateDate { get; set; }
    public byte[] File { get; set; }
}
