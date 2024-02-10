using Business_Layer.IServices;
using Business_Layer.Services;
using DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Innovura_TaskCraft.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
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

        [HttpGet("tasks")]
        public async Task<IActionResult> GetTasks()
        {
            var userID = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var emailID = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            var UserID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var tasks = await _taskManager.GetTaskByUserIdAsync(int.Parse(UserID));
            return Json(tasks.ToList()); ;
        }
        
        [HttpGet("labels")]
        public async Task<IActionResult> GetLabels()
        {
            var userID = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var emailID = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var labels = await _labelManager.GetLabelByUserIdAsync(int.Parse(userID));
            return Json(labels.ToList()); ;
        }
        [HttpPost("createlabel")]
        public async Task<IActionResult> createLabel(Label label)
        {
            var userID = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var emailID = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            label.UserId = int.Parse(userID);
            label.TimeSpan = DateTime.Now.TimeOfDay;
            var createdLabel = await _labelManager.CreateLabelAsync(label);
            
            return Ok( createdLabel);
        }

        [HttpGet("getTask/{id}")]
        public async Task<ActionResult<Task>> GetTaskById(int id)
        {
            var userID = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var emailID = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
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
            var userID = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var emailID = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            task.User = await _userManager.GetUserByIdAsync(int.Parse(userID));
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
            var userID = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var emailID = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            task.UserId = int.Parse(userID);
            var taskupdated = await _taskManager.UpdateTaskAsync(task);
            if (taskupdated == 0)
            {
                return NotFound();
            }
            return Ok(taskupdated);
        }

        [HttpDelete("taskdelete/{id}")]
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
