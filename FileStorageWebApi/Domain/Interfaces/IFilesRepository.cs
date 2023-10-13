using FileStorageWebApi.Domain.Models;

namespace FileStorageWebApi.Domain.Interfaces;

public interface IFilesRepository
{
    Task Upload(FileModel fileModel);

    Task<List<FileModel>> GetAll();

    Task<FileModel?> Get(Guid fileId);

    Task Update(FileModel fileModel);
}