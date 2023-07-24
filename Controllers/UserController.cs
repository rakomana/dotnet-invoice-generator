using learnApi.Models;
using learnApi.Data;
using Microsoft.AspNetCore.Mvc;

namespace learnApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly DataContextEF _context;

        public UserController(DataContextEF context)
        {
            _context = context;
        }

        public DataContextEF Context => _context;
    
        [HttpPost]
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
            }*/