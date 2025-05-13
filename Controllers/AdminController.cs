using BlogProjesi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogProjesi.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("IsAdmin") != "True")
                return Unauthorized();

            var posts = _context.Posts.Include(p => p.User).OrderByDescending(p => p.CreatedAt).ToList();
            return View(posts);
        }
    }
}
