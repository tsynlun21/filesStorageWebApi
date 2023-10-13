namespace FileStorageWebApi.Domain.Models;

public class FileDTOModel
{
    public Guid Id { get; set; }

    public string FileName { get; set; }

    public int OwnerId { get; set; }
}