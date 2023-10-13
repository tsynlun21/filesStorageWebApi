using System.Collections.Concurrent;
using FileStorageWebApi.Domain.Interfaces;
using FileStorageWebApi.Domain.Models;

namespace FileStorageWebApi.Services;

public class FilesProgressService
{
    private readonly IFilesRepository _filesRepository;
    public FilesProgressService(IFilesRepository filesRepository)
    {
        _filesRepository = filesRepository;
    }
    private static ConcurrentDictionary<Guid, int> FilesProgress { get; set; } = new ();

    public async Task<(bool FileIsUploading, string procent)> GetDownloadProgress(Guid fileId)
    {
        if (FilesProgress.TryGetValue(fileId, out int procent))
            return (true, procent + "%");
        else
        {
            var file = await _filesRepository.Get(fileId);

            if (file is not null)
                return (true, "100%");
        }

        return (false, string.Empty);

    }

    internal void SetFileProgress(Guid fileId, int procent)
    {
        FilesProgress.TryAdd(fileId, procent);
    }

    internal void RemoveFileProgress(Guid fileId)
    {
        FilesProgress.TryRemove(fileId, out _);
    }
}