using SchoolLearningSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLearningSystem.Domain.Interfaces
{
    public interface ICurriculumRepository
    {
        Task<IEnumerable<Curriculum>> GetAllAsync();
        Task<Curriculum> GetByIdAsync(int id);
        Task AddAsync(Curriculum curriculum);
        Task UpdateAsync(Curriculum curriculum);
        Task DeleteAsync(int id);
    }

}
