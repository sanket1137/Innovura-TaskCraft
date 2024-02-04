using Business_Layer.IServices;
using Innovura_TaskCraft.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Innovura_TaskCraft.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeEssentials _homeEssentials;
        private readonly ITaskManager _taskManager;
        private readonly ILabelManager _labelManager;

        public HomeController(ILogger<HomeController> logger,  ITaskManager taskManager, ILabelManager labelManager, IHomeEssentials homeEssentials)
        {
            _logger = logger;
            _taskManager = taskManager;
            _labelManager = labelManager;
            _homeEssentials = homeEssentials;
        }

        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("userId")))
            {
                return RedirectToAction("Login", "Accounts");
            }
            //_homeEssentials.Tasks = _taskManager.GetTaskByUserIdAsync(int.Parse(HttpContext.Session.GetString("userId"))).Result;
            ViewBag.Tasks = _taskManager.GetTaskByUserIdAsync(int.Parse(HttpContext.Session.GetString("userId"))).Result;
            ViewBag.Labels = _labelManager.GetLabelByUserIdAsync(int.Parse(HttpContext.Session.GetString("userId"))).Result;
            
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
