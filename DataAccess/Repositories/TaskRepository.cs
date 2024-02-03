using DataAccess.Data;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public TaskRepository(ApplicationDbContext dbContext)
        {
                _dbContext= dbContext;
        }
        public async Task<IEnumerable<TaskItem>> GetTasksAsync()
        {
            return await _dbContext.Tasks.ToListAsync();
        }
        public async Task<TaskItem> GetTaskByIdAsync(int id)
        {
            return await _dbContext.Tasks.FindAsync(id);
        }
        public async Task<int> CreateTaskAsync(TaskItem task)
        {
            _dbContext.Tasks.Add(task);
            await _dbContext.SaveChangesAsync();
            return task.Id;
        }
        public async Task<int> UpdateTaskAsync(TaskItem task)
        {
            _dbContext.Tasks.Update(task);
            return await _dbContext.SaveChangesAsync();
        }
        public async Task<int> DeleteTaskAsync(int id)
        {
            var task = await _dbContext.Tasks.FindAsync(id);

            if (task != null)
            {
                _dbContext.Tasks.Remove(task);
                return await _dbContext.SaveChangesAsync();
            }

            return 0;
        }
    }
}
