using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        [Authorize(Roles = "Administrator,Moderator")]
        public IActionResult EditPost(int id)
        {
            Post thisPost = _db.Posts.FirstOrDefault(x => x.PostId == id);
            return View(thisPost);
        }

        [Authorize(Roles = "Administrator,Moderator")]
        [HttpPost]
        public IActionResult EditPost(Post post)
        {
            _db.Entry(post).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Administrator,Moderator")]
		[HttpPost]
		public IActionResult DeletePost(int id)
		{
			Post thisPost = _db.Posts.FirstOrDefault(x => x.PostId == id);
			_db.Posts.Remove(thisPost);
			_db.SaveChanges();
			return RedirectToAction("Index");
		}

        public IActionResult Detail(int id)
        {
            Post thisPost = _db.Posts.Include(p => p.Comments).FirstOrDefault(x => x.PostId == id);
            return View(new PostComments(thisPost));
        }

        [HttpPost]
        public IActionResult AddComment(Comment comment)
        {
            _db.Comments.Add(comment);
            _db.SaveChanges();
            return RedirectToAction("Detail", new { id = comment.PostId });
        }


        [HttpPost]
        public IActionResult DeleteComment(int id)
        {
            Comment thisComment = _db.Comments.FirstOrDefault(x => x.CommentId == id);
            _db.Comments.Remove(thisComment);
            _db.SaveChanges();
            return Content(id.ToString());
        }
    }
}
