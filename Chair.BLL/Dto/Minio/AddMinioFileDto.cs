namespace Chair.BLL.Dto.Minio;

public class AddMinioFileDto
{
    public string FileName { get; set; }
    public MemoryStream FileData { get; set; }
}
