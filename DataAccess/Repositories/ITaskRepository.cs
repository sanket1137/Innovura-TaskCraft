using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskItem>> GetTasksAsync();
        Task<TaskItem> GetTaskByIdAsync(int id);
        Task<IList<TaskItem>> GetTaskByUserIdAsync(int id);
        Task<int> CreateTaskAsync(TaskItem task);
        Task<int> UpdateTaskAsync(TaskItem task);
        Task<int> DeleteTaskAsync(int id);
    }
}
