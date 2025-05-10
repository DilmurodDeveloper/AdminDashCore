using AdminDashCore.Data;
using AdminDashCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AdminDashCore.Pages.Admin.Messages
{
    public class DeleteModel : PageModel
    {
        private readonly AppDbContext _context;

        public DeleteModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public IActionResult OnPost()
        {
            var message = _context.Messages.Find(Message?.Id);

            if (message == null)
            {
                return NotFound();
            }

            _context.Messages.Remove(message);
            _context.SaveChanges();
            return RedirectToPage("Index");
        }
    }
}
