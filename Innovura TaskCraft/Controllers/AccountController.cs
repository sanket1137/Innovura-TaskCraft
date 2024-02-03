using Microsoft.AspNetCore.Mvc;

namespace Innovura_TaskCraft.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
