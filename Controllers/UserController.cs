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
    [Route("api/")]
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

        [Route("user/")]
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetTestUnAuthorise()
        {
            return Ok("Hello world from GetTestUnAuthorise");
        }

        [Route("user")]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> GetTestAuthorise()
        {
            return Ok("Hello world from GetTestAuthorise");
        }
    
        [HttpPost("user/quotation")]
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

        [HttpGet("user/quotation")]
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

// dotnet add package BCrypt.Net

// using BCrypt.Net;

// // Hash a password
// string password = "SecurePassword123"; // Replace with the actual password
// string hashedPassword = BCrypt.HashPassword(password);


// using BCrypt.Net;

// string hashedPassword = "hashed_password_from_database"; // Replace with the hashed password from your database
// string passwordAttempt = "SecurePassword123"; // Replace with the attempted password

// bool isPasswordValid = BCrypt.Verify(passwordAttempt, hashedPassword);

// if (isPasswordValid)
// {
//     // Password is correct
// }