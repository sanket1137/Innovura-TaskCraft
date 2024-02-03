using Business_Layer.IServices;
using DataAccess.Entities;
using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer.Services
{
    public class TaskManager : ITaskManager
    {
        private readonly ITaskRepository _taskService;

        public TaskManager(ITaskRepository taskService)
        {
            _taskService = taskService;
        }

        public async Task<IEnumerable<TaskItem>> GetTasksAsync()
        {
            return await _taskService.GetTasksAsync();
        }

        public async Task<TaskItem> GetTaskByIdAsync(int id)
        {
            return await _taskService.GetTaskByIdAsync(id);
        }

        public async Task<int> CreateTaskAsync(TaskItem task)
        {
            return await _taskService.CreateTaskAsync(task);
        }

        public async Task<int> UpdateTaskAsync(TaskItem task)
        {
            return await _taskService.UpdateTaskAsync(task);
        }

        public async Task<int> DeleteTaskAsync(int id)
        {
            return await _taskService.DeleteTaskAsync(id);
        }
    }
}
