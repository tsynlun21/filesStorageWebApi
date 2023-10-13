using FileStorageWebApi.Domain.Models;

namespace FileStorageWebApi.Domain.Interfaces;

public interface IUsersService
{
    Task<string> AddUsers(List<string> users);

    UserModel GetCurrentUser();

    Task<(bool UserExists, UserModel? User)> SetCurrentUser(int userId);
}