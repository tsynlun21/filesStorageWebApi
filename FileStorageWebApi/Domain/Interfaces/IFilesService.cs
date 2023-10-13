using FileStorageWebApi.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace FileStorageWebApi.Domain.Interfaces;

public interface IFilesService
{
    Task<string> UploadFilesAsync(params IFormFile[] files);

    Task<(FileStreamResult? filesStream, bool atLeastOneFileIsNotOwned)> DownloadFiles(List<Guid> guids);

    Task<List<FileDTOModel>> GetFilesAsync();
}