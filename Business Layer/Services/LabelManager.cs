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
    public class LabelManager: ILabelManager
    {
        private readonly ILabelRepository _labelRepository;
        public LabelManager(ILabelRepository labelRepository)
        {
            _labelRepository = labelRepository;
        }

        public async Task<IEnumerable<Label>> GetLabelAsync()
        {
            return await _labelRepository.GetLabelAsync();
        }

        public async Task<Label> GetLabelByIdAsync(int id)
        {
            return await _labelRepository.GetLabelByIdAsync(id);
        }
        public async Task<IList<Label>> GetLabelByUserIdAsync(int id)
        {
            return await _labelRepository.GetLabelByUserIdAsync(id);
        }
        public async Task<int> CreateLabelAsync(Label label)
        {
            return await _labelRepository.CreateLabelAsync(label);
        }

        public async Task<int> UpdateLabelAsync(Label label)
        {
            return await _labelRepository.UpdateLabelAsync(label);
        }

        public async Task<int> DeleteLabelAsync(int id)
        {
            return await _labelRepository.DeleteLabelAsync(id);
        }
    }
}
