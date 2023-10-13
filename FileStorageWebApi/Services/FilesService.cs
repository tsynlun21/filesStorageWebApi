using System.Text;
using FileStorageWebApi.Domain.Interfaces;
using FileStorageWebApi.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using ZipArchive = SharpCompress.Archives.Zip.ZipArchive;

namespace FileStorageWebApi.Services;

public class FilesService : IFilesService
{
    private readonly IFilesRepository _repository;
    private readonly FilesProgressService _filesProgressService;
    private readonly IUsersService _userService;
    private const string FILE_MIME_TYPE = "application/octet-stream";
    private const string ZIP_MIME_TYPE = "application/zip";

    public FilesService(IFilesRepository repository, FilesProgressService filesProgressService, IUsersService usersService)
    {
        _repository = repository;
        _filesProgressService = filesProgressService;
        _userService = usersService;
    }

    public async Task<string> UploadFilesAsync(params IFormFile[] files)
    {
        if (_userService.GetCurrentUser() is null)
            return "Нельзя загрузить файл - отсутсвует владелец файла.";

        var sb = new StringBuilder();
        foreach (var file in files)
        {
            var fileDetails = await UploadFileAsync(file);
            sb.Append($"Файл {fileDetails.FileName} был успешно загружен, его id - {fileDetails.FileId}\n");
        }

        return sb.ToString();
    }

    public async Task<List<FileDTOModel>> GetFilesAsync()
    {
        var resultList = new List<FileDTOModel>();
        var files = (await _repository.GetAll()).Where(file => file.OwnerId == _userService.GetCurrentUser().Id).ToList();

        foreach (var file in files)
        {
            resultList.Add(new FileDTOModel(){FileName = file.FileName, Id = file.Id, OwnerId = file.OwnerId});
        }

        return resultList;
    }

    public async Task<(FileStreamResult? filesStream, bool atLeastOneFileIsNotOwned)> DownloadFiles(List<Guid> guids)
    {
        var results = new List<FileStreamResult?>();
        foreach (var guid in guids)
        {
            var file = await DownloadFile(guid);
            if (file.isOwner == false)
            {
                return (null, true);
            }

            if (file.FileExists)
            {
                results.Add(file.FileStream);
            }
        }

        if (results.Count > 1)
        {
            var zip = CreateZipStream(results);
            return (zip, false);
        }

        if (results.Count == 1)
        {
            return (results[0], false);
        }
        return (null, false);
    }

    public FileStreamResult CreateZipStream(List<FileStreamResult?> files)
    {
        
        using (var archive = ZipArchive.Create())
        {
            foreach (var fileModel in files)
            {
                var i = 0;
                while (archive.Entries.Any(entry => entry.Key == fileModel!.FileDownloadName.Replace(".", (i != 0 ? $" ({i})." : "."))))
                {
                    i++;
                }

                archive.AddEntry(fileModel!.FileDownloadName.Replace(".", (i != 0 ? $" ({i})." : ".")), fileModel.FileStream);
            }

            MemoryStream ms = new();
            
            archive.SaveTo(ms);
            var res = new FileStreamResult(ms, ZIP_MIME_TYPE)
            {
                FileDownloadName = "files.zip",
                FileStream =
                {
                    Position = 0
                }
            };
            return res;
        }
    }

    private async Task<(Guid FileId, string FileName)> UploadFileAsync(IFormFile file)
    {
        var currentUser = _userService.GetCurrentUser();

        var fileModel = new FileModel()
        {
            FileName = file.FileName,
            Content = Enumerable.Empty<byte>().ToArray(),
            Id = new(),
            OwnerId = currentUser.Id
        };

        await _repository.Upload(fileModel);
        await Upload(file, fileModel.Id);

        return (fileModel.Id, fileModel.FileName);
    }

    private async Task<(bool FileExists, FileStreamResult? FileStream, bool? isOwner)> DownloadFile(Guid id)
    {
        var file = await _repository.Get(id);

        if (file is null) 
            return (false, null, null);

        if (file.OwnerId != _userService.GetCurrentUser().Id)
            return (false, null, false);

        var stream = new MemoryStream(file.Content);
        
        return (true, new(stream, FILE_MIME_TYPE)
        {
            FileDownloadName = file.FileName
        }, true);
    }

    private async Task Upload(IFormFile file, Guid id)
    {
        byte[] buffer = new byte[528 * 1024];
        long totalSizeInBytes = file.Length;
        long totalReadBytes = 0;

        const int FILE_START = 0;

        var fileModel = await _repository.Get(id);

        using (var stream = file.OpenReadStream())
        {
            int readBytes;

            while ((readBytes = await stream.ReadAsync(buffer, FILE_START, buffer.Length)) > 0)
            {
                fileModel.Content = fileModel.Content.Concat(buffer.Take(readBytes)).ToArray();
                await _repository.Update(fileModel);
                totalReadBytes += readBytes;

                var currentProgress = (int)((float)totalReadBytes / totalSizeInBytes * 100.0);
                _filesProgressService.SetFileProgress(fileModel.Id, currentProgress);
            }
        }

        _filesProgressService.RemoveFileProgress(fileModel.Id);

        await _repository.Update(fileModel);
    }
}