using Microsoft.AspNetCore.Mvc;
using Portfolio.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Portfolio.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Projects()
        {
            List<Repository> topStarredRepos = await ApiContext.TopThreeRepos();
            return View(topStarredRepos);
        }

        public IActionResult Contact()
        {
            return View();
        }
    }
}
