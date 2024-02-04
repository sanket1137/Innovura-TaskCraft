using DataAccess.Data;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class LabelRepository: ILabelRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public LabelRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<Label>> GetLabelAsync()
        {
            return await _dbContext.Label.ToListAsync();
        }
        public async Task<Label> GetLabelByIdAsync(int id)
        {
            return await _dbContext.Label.FindAsync(id);
        }
        public async Task<IList<Label>> GetLabelByUserIdAsync(int id)
        {
            return await _dbContext.Label.Where(t => t.UserId == id).ToListAsync();
        }
        public async Task<int> CreateLabelAsync(Label label)
        {
            _dbContext.Label.Add(label);
            await _dbContext.SaveChangesAsync();
            return label.Id;
        }
        public async Task<int> UpdateLabelAsync(Label label)
        {
            _dbContext.Label.Update(label);
            return await _dbContext.SaveChangesAsync();
        }
        public async Task<int> DeleteLabelAsync(int id)
        {
            var task = await _dbContext.Label.FindAsync(id);

            if (task != null)
            {
                _dbContext.Label.Remove(task);
                return await _dbContext.SaveChangesAsync();
            }

            return 0;
        }
    }
}
