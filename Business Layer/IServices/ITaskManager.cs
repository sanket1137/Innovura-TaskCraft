using DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business_Layer.IServices
{
    public interface ITaskManager
    {
        Task<IEnumerable<TaskItem>> GetTasksAsync();
        Task<TaskItem> GetTaskByIdAsync(int id);
        Task<int> CreateTaskAsync(TaskItem task);
        Task<int> UpdateTaskAsync(TaskItem task);
        Task<int> DeleteTaskAsync(int id);
    }
}
