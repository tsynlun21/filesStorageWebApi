using System.Text;
using FileStorageWebApi.Domain.Interfaces;
using FileStorageWebApi.Domain.Models;

namespace FileStorageWebApi.Services;

public class UsersService : IUsersService
{
    private static UserModel? _currentUser;

    private readonly IUsersRepository _usersRepository;
    private readonly IFilesRepository _filesRepository;

    public UsersService(IUsersRepository usersRepository, IFilesRepository filesRepository)
    {
        _usersRepository = usersRepository;
        _filesRepository = filesRepository;
    }


    public async Task<string> AddUsers(List<string> usernames)
    {
        var sb = new StringBuilder();
        foreach (var username in usernames)
        {
            var newUser = new UserModel()
            {
                UserName = username
            };

            var userId = await _usersRepository.AddUser(newUser);

            sb.Append($"Пользователь {username} успешно добавлен в базу данных, его id - {userId}\n");
        }

        return sb.ToString();
    }

    public UserModel GetCurrentUser()
    {
        if (_currentUser is null)
        {
            _currentUser = _usersRepository.TryGetFirstUser();
            return _currentUser;
        }

        return _currentUser;
    }

    public async Task<(bool UserExists, UserModel? User)> SetCurrentUser(int userId)
    {
        var user = await _usersRepository.GetUser(userId);

        if (user is null)
            return (false, null);

        _currentUser = user;

        return (true, _currentUser);
    }
}