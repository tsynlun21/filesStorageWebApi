using Microsoft.EntityFrameworkCore;

namespace FileStorageWebApi.Domain.Models;

public sealed class DataContext: DbContext
{
    public DbSet<FileModel> Files { get; set; }
    public DbSet<UserModel> Users { get; set; }
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
}