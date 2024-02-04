using Business_Layer.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public TaskCraftController(ITaskManager taskManager)
        {
            _taskManager = taskManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("tasks")]
        public async Task<IActionResult> GetAllTasks()
        {
            if(HttpContext.Session.GetString("userId")==null)
                return RedirectToAction("Login", "Account");
            var tasks = await _taskManager.GetTaskByUserIdAsync(int.Parse(HttpContext.Session.GetString("userId")));
            return Json(tasks.ToList()); ;
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
