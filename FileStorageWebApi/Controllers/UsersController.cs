using FileStorageWebApi.Domain.Interfaces;
using FileStorageWebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FileStorageWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;
        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpPost("user")]
        public async Task<IActionResult> AddUsers([FromQuery] string[] usernames)
        {
            var res = await _usersService.AddUsers(usernames.ToList());

            return Content(res);
        }

        [HttpPut("user/set-сurrent/{id}")]
        public async Task<IActionResult> SetCurrentUser([FromRoute] int id)
        {
            var res = await _usersService.SetCurrentUser(id);

            if (!res.UserExists)
                return NotFound();

            return Ok($"Текущий пользователь - {res.User.UserName}");
        }
    }
}
