using FileStorageWebApi.Domain.Interfaces;
using FileStorageWebApi.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FileStorageWebApi.Repositories;

public class UsersRepository: IUsersRepository
{
    private readonly DataContext _context;
    public UsersRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<int> AddUser(UserModel user)
    {
        var res = await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return res.Entity.Id;
    }

    public async Task DeleteUser(UserModel user)
    {
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }

    public UserModel? TryGetFirstUser()
    {
        return _context.Users.FirstOrDefault();
    }

    public async Task<UserModel?> GetUser(int id)
    {
        return await _context.Users.FirstOrDefaultAsync(user => user.Id == id);
    }

    public async Task UpdateUser(UserModel user)
    {
         _context.Update(user);
         await _context.SaveChangesAsync();
    }
}