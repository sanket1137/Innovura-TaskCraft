using Business_Layer.IServices;
using Business_Layer.Services;
using DataAccess.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Innovura_TaskCraft.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    public class TaskCraftController : Controller
    {
        private readonly ITaskManager _taskManager;
        private readonly ILabelManager _labelManager;
        private readonly IUserManager _userManager;

        public TaskCraftController(ITaskManager taskManager, ILabelManager labelManager, IUserManager userManager)
        {
            _taskManager = taskManager;
            _labelManager = labelManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("tasks")]
        public async Task<IActionResult> GetTasks()
        {
            if(HttpContext.Session.GetString("userId")==null)
                return RedirectToAction("Login", "Account");
            var tasks = await _taskManager.GetTaskByUserIdAsync(int.Parse(HttpContext.Session.GetString("userId")));
            return Json(tasks.ToList()); ;
        }
        
        [HttpGet("labels")]
        public async Task<IActionResult> GetLabels()
        {
            if (HttpContext.Session.GetString("userId") == null)
                return RedirectToAction("Login", "Account");
            var labels = await _labelManager.GetLabelByUserIdAsync(int.Parse(HttpContext.Session.GetString("userId")));
            return Json(labels.ToList()); ;
        }
        [HttpPost("createlabel")]
        public async Task<IActionResult> createLabel(Label label)
        {
            var createdLabel = await _labelManager.CreateLabelAsync(label);
            
            return Ok( createdLabel);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Task>> GetTaskById(int id)
        {
            var task = await _taskManager.GetTaskByIdAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }
        [HttpPost("taskcreate")]
        public async Task<IActionResult> createTask(TaskItem task)
        {
            if (HttpContext.Session.GetString("userId") == null)
                return RedirectToAction("Login", "Account");
            task.User = await _userManager.GetUserByIdAsync(int.Parse(HttpContext.Session.GetString("userId")));
            task.CreatedDate = DateTime.Now;
            var isTaskCreated = await _taskManager.CreateTaskAsync(task);

            if (isTaskCreated == 0)
            {
                return NotFound();
            }
            return Ok(isTaskCreated);
        }

        [HttpPatch("taskupdate")]
        public async Task<ActionResult<TaskItem>> TaskUpdate(TaskItem task)
        {
            if (HttpContext.Session.GetString("userId") == null)
                return RedirectToAction("Login", "Account");
            task.UserId = int.Parse(HttpContext.Session.GetString("userId"));
            var taskupdated = await _taskManager.UpdateTaskAsync(task);
            if (taskupdated == 0)
            {
                return NotFound();
            }
            return Ok(taskupdated);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Task>> DeleteTask(int id)
        {
            var trashed = await _taskManager.DeleteTaskAsync(id);
            if (trashed == 0)
            {
                return NotFound();
            }

            return Ok(trashed);
        }
    }
}
