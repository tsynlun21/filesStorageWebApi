using FileStorageWebApi.Domain.Models;

namespace FileStorageWebApi.Domain.Interfaces;

public interface IUsersRepository
{
    Task<int> AddUser(UserModel user);

    Task DeleteUser(UserModel user);

    UserModel? TryGetFirstUser();

    Task<UserModel?> GetUser(int id);

    Task UpdateUser(UserModel user);
}