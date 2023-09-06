using learnApi.Models;
using learnApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace learnApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly DataContextEF _context;
        private readonly IConfiguration _configuration;

        public UserController(DataContextEF context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public DataContextEF Context => _context;

                [Route("GetTestUnAuthorise")]
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetTestUnAuthorise()
        {
            return Ok("Hello world from GetTestUnAuthorise");
        }

        [Route("GetTestAuthorise")]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> GetTestAuthorise()
        {
            return Ok("Hello world from GetTestAuthorise");
        }


        [Route("CheckLogin")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CheckLogin([FromBody] LoginRequest model)
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
    
        [HttpPost("/api/user/quotation")]
        public async Task<ActionResult<List<Quotation>>> AddQuotation(User request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Create User instance from the received model
            User user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName
            };

            // Create Quotation instance from the received model
            Quotation quotation = new Quotation
            {
                EntityName = request.Quotation.EntityName,
                EntityDescription = request.Quotation.EntityDescription,
                DateCreated = request.Quotation.DateCreated,
                QuotationNumber = request.Quotation.QuotationNumber,
                // Assign the UserId to the Quotation (foreign key relationship)
                UserId = user.Id
            };

            user.Quotation = quotation;

            // Add the User and Quotation to the context and save changes
            _context.users.Add(user);
            _context.SaveChanges();

            return Ok(new { message = "User and Quotation saved successfully." });
        }

        [HttpGet("/api/quotations")]
        public async Task<ActionResult<List<Quotation>>> GetQuotations() 
        {
            var quotations = _context.quotations.ToList();

            return Ok(quotations);
        }
    }
}
/*
            {
                "firstName": "John",
                "lastName": "Doe",
                "quotation": {
                    "entityName": "Sample Entity",
                    "entityDescription": "Sample description",
                    "dateCreated": "2023-05-10",
                    "quotationNumber": "Q12345"
                }
            }
            
[AllowAnonymous] -  is used to specify that a particular controller action or method should allow unauthenticated
[Authorize] // Requires authentication
*/