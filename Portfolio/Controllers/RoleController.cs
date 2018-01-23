using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Portfolio.Models;

namespace Portfolio.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class RoleController : Controller
    {
        private readonly PortfolioDbContext _db;
        private readonly UserManager<PortfolioUser> _userManager;
        private readonly SignInManager<PortfolioUser> _signInManager;
        private readonly RoleManager<PortfolioRole> _roleManager;

        public RoleController(UserManager<PortfolioUser> userManager, SignInManager<PortfolioUser> signInManager, RoleManager<PortfolioRole> roleManager, PortfolioDbContext db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _db = db;
        }

        public IActionResult Index()
        {
            return View(_db.Roles.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(IFormCollection collection)
        {
            PortfolioRole role = new PortfolioRole
            {
                Name = collection["RoleName"]
            };
            IdentityResult result = await _roleManager.CreateAsync(role);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(string roleName)
        {
            var thisRole = _db.Roles.Where(r => r.Name == roleName).FirstOrDefault();
            _db.Roles.Remove(thisRole);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult ManageUserRoles()
        {
            ViewBag.RoleList = new SelectList(_db.Roles, "Name", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RoleAddToUser(string UserName, string RoleName)
        {
            var user = _db.Users.Where(u => u.UserName == UserName).FirstOrDefault();
            await _userManager.AddToRoleAsync(user, RoleName);

            ViewBag.RoleList = new SelectList(_db.Roles, "Name", "Name");

            return View("ManageUserRoles");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetRoles(string UserName)
        {
            if (!string.IsNullOrWhiteSpace(UserName))
            {
                PortfolioUser user = _db.Users.Where(u => u.UserName == UserName).FirstOrDefault();


                ViewBag.RolesForThisUser = await _userManager.GetRolesAsync(user);

                ViewBag.RoleList = new SelectList(_db.Roles, "Name", "Name");
            }

            return View("ManageUserRoles");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRoleForUser(string UserName, string RoleName)
        {
            PortfolioUser user = _db.Users.Where(u => u.UserName == UserName).FirstOrDefault();

            if (await _userManager.IsInRoleAsync(user, RoleName))
            {
                await _userManager.RemoveFromRoleAsync(user, RoleName);
            }

            ViewBag.RoleList = new SelectList(_db.Roles, "Name", "Name");

            return View("ManageUserRoles");
        }
    }
}
