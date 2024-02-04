using Business_Layer.IServices;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
        public async Task<ActionResult<IEnumerable<Task>>> GetAllTasks()
        {
            var tasks = await _taskManager.GetTasksAsync();
            return Ok(tasks);
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
    }
}
