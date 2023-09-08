using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using learnApi.Data;
using learnApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace learnApi.Controllers
{
    [ApiController]
        [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly DataContextEF _context;
        private readonly IConfiguration _configuration;

        public AuthController(DataContextEF context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public DataContextEF Context => _context;

        [Route("login")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            var username = model.Username;
            var password = model.Password;
            
            string message;
            
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return BadRequest("Username and password are required.");
            } else 
            {
                var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("Username", username)
                    };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(10),
                    signingCredentials: signIn);
                message = "Login Success";
                password = new JwtSecurityTokenHandler().WriteToken(token);
            }

            var response = new {
                Token = password,
                Message = message
            };
            
            return Ok(response);
        }

        [Route("register")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        {
            try
            {
                var passwordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

                var newUser = new User
                {
                    UserName = model.Username,
                    Password = passwordHash,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };

                _context.users.Add(newUser);
                await _context.SaveChangesAsync();

                var response = new { Message = "User created successfully." };

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Handle exceptions or validation errors appropriately.
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [Route("reset-password")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromBody] LoginRequest model)
        {
            var username = model.Username;
            var password = model.Password;
            
            string message;
            
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return BadRequest("Username and password are required.");
            } else 
            {
                var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("Username", username)
                    };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(10),
                    signingCredentials: signIn);
                message = "Login Success";
                password = new JwtSecurityTokenHandler().WriteToken(token);
            }

            var response = new {
                Token = password,
                Message = message
            };
            
            return Ok(response);
        }

        [Route("verify-email")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> VerifyEmail([FromBody] LoginRequest model)
        {
            var username = model.Username;
            var password = model.Password;
            
            string message;
            
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return BadRequest("Username and password are required.");
            } else 
            {
                var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("Username", username)
                    };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(10),
                    signingCredentials: signIn);
                message = "Login Success";
                password = new JwtSecurityTokenHandler().WriteToken(token);
            }

            var response = new {
                Token = password,
                Message = message
            };
            
            return Ok(response);
        }
    }
}