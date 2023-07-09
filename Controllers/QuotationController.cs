using learnApi.Data;
using learnApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace learnApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuotationController : ControllerBase
    {
        private readonly DataContextEF _context;

        public QuotationController(DataContextEF context)
        {
            _context = context;
        }

        public DataContextEF Context => _context;
    
        [HttpPost]
        public async Task<ActionResult<List<Quotation>>> AddQuotation(int Id, Quotation request)
        {
            var user = _context.users.Find(Id);

            var quotation = new Quotation {
                EntityName = request.EntityName,
                QuotationNumber = request.QuotationNumber,
                EntityDescription = request.EntityDescription,
                DateCreated = request.DateCreated,
                User = user,
            };

            var items = request.Items.Select(i => new Item{ItemDescription = i.ItemDescription}).ToList();

            quotation.User = user;
            quotation.Items = items;

            _context.quotations.Add(quotation);

            await _context.SaveChangesAsync();

            return Ok(await _context.quotations.Include(c => c.User).Include(c => c.Items).ToListAsync());
        }
    }
}