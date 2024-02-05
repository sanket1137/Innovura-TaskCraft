using Business_Layer.IServices;
using DataAccess.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Innovura_TaskCraft.Controllers
{
    public class AccountsController : Controller
    {
        private readonly IUserManager _userManager;
        public AccountsController(IUserManager userManager)
        {
            _userManager = userManager;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("userId")) && HttpContext.Session.GetString("userId") == user.Id.ToString())
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                User tempUser =  await _userManager.GetUserByEmailAsync(user.UserEmailId);
                
                if (tempUser != null && await _userManager.GetUserByEmailAndPasswordAsync(user.UserEmailId, user.Password))
                {
                    HttpContext.Session.SetString("userId", tempUser.Id.ToString());
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("Error", "Invalid login attempt. Please check your email and password.");
                    return View(user);
                }
            }
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            
                User tempUser = await _userManager.GetUserByEmailAsync(user.UserEmailId);
                if (tempUser == null)
                {
                    var createdUser=_userManager.AddUserAsync(user);
                    HttpContext.Session.SetString("userId", createdUser.Id.ToString());
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("Error", "User Already Registered. Try to Login or Try with another Id");
                    return View(user);
                }
            
        }
    }
}
