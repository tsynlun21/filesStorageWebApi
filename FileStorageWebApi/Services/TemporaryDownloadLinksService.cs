using System.Collections.Concurrent;
using FileStorageWebApi.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FileStorageWebApi.Services;

public class TemporaryDownloadLinksService
{
    private readonly IFilesService _filesService;
    public TemporaryDownloadLinksService(IFilesService filesService)
    {
        _filesService = filesService;
    }
    private static ConcurrentDictionary<Guid, FileStreamResult> LinksDictionary { get; set; } = new();

    public async Task<(bool FileExists, Guid? Link, bool nowOwnedFile)> AddLink(List<Guid> ids)
    {
        var file = await _filesService.DownloadFiles(ids);

        if (file.atLeastOneFileIsNotOwned)
        {
            return (true, null, true);
        }

        if (file.filesStream is null)
            return (false, null, false);

        var downloadLink = Guid.NewGuid();
        LinksDictionary.TryAdd(downloadLink, file.filesStream);

        return (true, downloadLink, false);
    }

    public (bool LinkIsValid, FileStreamResult? FilesStream) DownloadFileByLink(Guid link)
    {
        if (!LinksDictionary.TryRemove(link, out var fileStream))
        {
            return (false, null);
        }
        else
        {
            return (true,  fileStream);
        }
    }
}