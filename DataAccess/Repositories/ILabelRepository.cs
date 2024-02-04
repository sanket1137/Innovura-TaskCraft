using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public interface ILabelRepository
    {
        Task<IEnumerable<Label>> GetLabelAsync();
        Task<Label> GetLabelByIdAsync(int id);
        Task<IList<Label>> GetLabelByUserIdAsync(int id);
        Task<int> CreateLabelAsync(Label label);
        Task<int> UpdateLabelAsync(Label label);
        Task<int> DeleteLabelAsync(int id);
    }
}
