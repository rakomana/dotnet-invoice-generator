using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using learnApi.Models;
using learnApi.Service.UserService;

namespace learnApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

       [HttpGet]
       public async Task<ActionResult<List<User>>> GetUsers()
       {
            var result = await _userService.GetUsers();

            return Ok(result);
       }

       [HttpGet("{id}")]
       public async Task<ActionResult<List<User>>> GetSingleUser(int id)
       {
            var result = _userService.GetSingleUser(id);

            if(result is null)
                return NotFound("user is not found");
            return Ok(result);
       }

        [HttpPost]
       public async Task<ActionResult<List<User>>> AddUser(User user)
       {
            var result = _userService.AddUser(user);

            if(result is null)
                return NotFound("User not found");

            return Ok(result);
       }

        [HttpPut("{id}")]
       public async Task<ActionResult<List<User>>> UpdateUser(int id, User request)
       {
            var result = _userService.UpdateUser(id, request);

            if(result is null)
                return NotFound("user not found");

            return Ok(result);
       }
    }
}