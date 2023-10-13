using FileStorageWebApi.Domain.Interfaces;
using FileStorageWebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FileStorageWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FilesController : ControllerBase
    {
        private readonly IFilesService _fileService;
        private readonly FilesProgressService _filesProgressService;
        private readonly TemporaryDownloadLinksService _linksService;
        
        public FilesController(IFilesService fileService, FilesProgressService filesProgressService, TemporaryDownloadLinksService linksService)
        {
            _fileService = fileService;
            _filesProgressService = filesProgressService;
            _linksService = linksService;
            
        }

        [HttpPost("upload")]
        public async Task<string> UploadFiles(params IFormFile[] files)
        {
            return await _fileService.UploadFilesAsync(files);
        }

        [HttpGet("download")]
        public async Task<IActionResult> Download([FromQuery] params Guid[] guids)
        {
            var result = await _fileService.DownloadFiles(guids.ToList());

            if (result.atLeastOneFileIsNotOwned)
                return Forbid();

            if (result.filesStream is not null)
                return result.filesStream;

            return NotFound();
        }

        [HttpGet("files")]
        public async Task<IActionResult> GetAllFiles()
        {
            var result = await _fileService.GetFilesAsync();

            if (result.Count > 0)
                return Ok(result);

            return NotFound();
        }

        [HttpGet("progress/{guid}")]
        public async Task<IActionResult> GetDownloadProgress([FromRoute] Guid guid)
        {
            var result = await _filesProgressService.GetDownloadProgress(guid);

            if (result.FileIsUploading)
                return Content($"‘айл {guid} загружен на {result.procent}");

            return NotFound();
        }

        [HttpGet("link")]
        public async Task<IActionResult> GetLinkForDownloading([FromQuery] params Guid[] fileGuid)
        {
            var result = await _linksService.AddLink(fileGuid.ToList());

            if (result.nowOwnedFile)
                return Forbid();

            if (result.FileExists)
                return Content($"—сылка на скачивание - {result.Link}");
            else
                return NotFound();
        }

        [HttpGet("download/{link}")]
        public async Task<IActionResult> DownloadFileByLink([FromRoute] Guid link)
        {
            var result = _linksService.DownloadFileByLink(link);

            if (!result.LinkIsValid)
            {
                return BadRequest();
            }
            else
            {
                return result.FilesStream!;
            }
        }
    }
}