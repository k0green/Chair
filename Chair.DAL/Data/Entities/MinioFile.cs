namespace Chair.DAL.Data.Entities;

public class MinioFile : BaseEntity
{
    public string Name { get; set; }
    public DateTime CreateDate { get; set; }
    public string Url { get; set; }
}