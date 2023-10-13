using FileStorageWebApi.Domain.Interfaces;
using FileStorageWebApi.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FileStorageWebApi.Repositories;

public class FilesRepository: IFilesRepository
{
    private readonly DataContext _context;
    public FilesRepository(DataContext context)
    {
        _context = context;
    }

    public async Task Upload(FileModel fileModel)
    {
        await _context.Files.AddAsync(fileModel);
        await _context.SaveChangesAsync();
    }

    public async Task<List<FileModel>> GetAll()
    {
        return await _context.Files.AsNoTracking().ToListAsync();
    }

    public async Task<FileModel?> Get(Guid fileId) => await _context.Files.FirstOrDefaultAsync(model => model.Id == fileId);

    public async Task Update(FileModel fileModel)
    {
        _context.Files.Update(fileModel);
        await _context.SaveChangesAsync();
    }
}