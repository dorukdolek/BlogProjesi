using BlogProjesi.Data;
using BlogProjesi.Models;
using BlogProjesi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogProjesi.Controllers
{
    public class PostController : Controller
    {
        private readonly AppDbContext _context;

        public PostController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var posts = _context.Posts
                .Include(p => p.User)
                .Include(p => p.Category) 
                .ToList();
            return View(posts);
        }


        public IActionResult Details(int id)
        {
            var post = _context.Posts
                .Include(p => p.User)
                .Include(p => p.Comments)
                .ThenInclude(c => c.User)
                .FirstOrDefault(p => p.Id == id);

            if (post == null) return NotFound();

            var viewModel = new CommentViewModel
            {
                Post = post,
                Comments = post.Comments.OrderByDescending(c => c.CreatedAt).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddComment(int id, CommentViewModel model)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            var post = _context.Posts.Include(p => p.Comments).FirstOrDefault(p => p.Id == id);
            if (post == null) return NotFound();

            if (ModelState.IsValid)
            {
                var comment = new Comment
                {
                    Content = model.Content,
                    CreatedAt = DateTime.Now,
                    PostId = id,
                    UserId = userId.Value
                };

                _context.Comments.Add(comment);
                _context.SaveChanges();
                return RedirectToAction("Details", new { id });
            }

            model.Post = post;
            model.Comments = post.Comments.OrderByDescending(c => c.CreatedAt).ToList();
            return View("Details", model);
        }

        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(PostCreateViewModel model)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                ModelState.AddModelError("", "Oturum süreci dolmuş.");
                ViewBag.Categories = _context.Categories.ToList();
                return View(model);
            }

            if (ModelState.IsValid)
            {
                var post = new Post
                {
                    Title = model.Title,
                    Content = model.Content,
                    CreatedAt = DateTime.Now,
                    UserId = userId.Value,
                    CategoryId = model.CategoryId ?? 0
                };

                _context.Posts.Add(post);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Categories = _context.Categories.ToList();
            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var post = _context.Posts.Find(id);
            if (post == null) return NotFound();

            var userId = HttpContext.Session.GetInt32("UserId");
            var isAdmin = HttpContext.Session.GetString("IsAdmin") == "True";

            if (post.UserId != userId && !isAdmin)
                return Unauthorized();

            ViewBag.Categories = _context.Categories.ToList();
            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Post post)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var isAdmin = HttpContext.Session.GetString("IsAdmin") == "True";

            var originalPost = _context.Posts.AsNoTracking().FirstOrDefault(p => p.Id == post.Id);
            if (originalPost == null) return NotFound();

            if (originalPost.UserId != userId && !isAdmin)
                return Unauthorized();

            post.UserId = originalPost.UserId;
            post.CreatedAt = originalPost.CreatedAt;

            if (ModelState.IsValid)
            {
                _context.Posts.Update(post);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Categories = _context.Categories.ToList();
            return View(post);
        }

        public IActionResult Delete(int id)
        {
            var post = _context.Posts.Find(id);
            if (post == null) return NotFound();

            var userId = HttpContext.Session.GetInt32("UserId");
            var isAdmin = HttpContext.Session.GetString("IsAdmin") == "True";

            if (post.UserId != userId && !isAdmin)
                return Unauthorized();

            return View(post);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var post = _context.Posts.Find(id);
            if (post == null) return NotFound();

            _context.Posts.Remove(post);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
