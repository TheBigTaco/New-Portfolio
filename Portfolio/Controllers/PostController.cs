using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Portfolio.Models;

namespace Portfolio.Controllers
{
    public class PostController : Controller
    {
        private readonly PortfolioDbContext _db;
        private readonly UserManager<PortfolioUser> _userManager;
        private readonly SignInManager<PortfolioUser> _signInManager;

        public PostController(UserManager<PortfolioUser> userManager, SignInManager<PortfolioUser> signInManager, PortfolioDbContext db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _db = db;
        }
        public IActionResult Index()
        {
            return View(_db.Posts.ToList());
        }

        [Authorize(Roles = "Administrator,Moderator")]
        public IActionResult CreatePost()
        {
            return View();
        }

        [Authorize(Roles = "Administrator,Moderator")]
        [HttpPost]
        public IActionResult CreatePost(Post post)
        {
            _db.Posts.Add(post);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
