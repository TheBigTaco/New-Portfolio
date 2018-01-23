using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.Models;
using Portfolio.ViewModels;

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
            post.Date = DateTime.Today;
            _db.Posts.Add(post);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Detail(int PostId)
        {
            Post thisPost = _db.Posts.Include(p => p.Comments).FirstOrDefault(x => x.PostId == PostId);
            return View(new PostComments(thisPost));
        }

        [HttpPost]
        public IActionResult Detail(Comment comment)
        {
            _db.Comments.Add(comment);
            _db.SaveChanges();
            return RedirectToAction("Detail", new { PostId = comment.PostId });
        }
    }
}
