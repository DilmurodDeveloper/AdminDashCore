using AdminDashCore.Data;
using AdminDashCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AdminDashCore.Pages.Admin.Messages
{
    public class DetailsModel : PageModel
    {
        private readonly AppDbContext _context;

        public DetailsModel(AppDbContext context)
        {
            _context = context;
        }

        public Message? Message { get; set; }

        public IActionResult OnGet(int id)
        {
            Message = _context.Messages.Include(m => m.Client).FirstOrDefault(m => m.Id == id);

            if (Message == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
