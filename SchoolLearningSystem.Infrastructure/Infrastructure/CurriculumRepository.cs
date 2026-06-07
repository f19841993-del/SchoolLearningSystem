using Microsoft.EntityFrameworkCore;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces;
using SchoolLearningSystem.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLearningSystem.Infrastructure.Infrastructure
{
    public class CurriculumRepository : ICurriculumRepository
    {
        private readonly AppDbContext _context;

        public CurriculumRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Curriculum>> GetAllAsync()
        {
            return await _context.Curriculums.ToListAsync();
        }

        public async Task<Curriculum> GetByIdAsync(int id)
        {
            return await _context.Curriculums.FindAsync(id);
        }

        public async Task AddAsync(Curriculum curriculum)
        {
            await _context.Curriculums.AddAsync(curriculum);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Curriculum curriculum)
        {
            _context.Curriculums.Update(curriculum);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var curriculum = await _context.Curriculums.FindAsync(id);
            if (curriculum != null)
            {
                _context.Curriculums.Remove(curriculum);
                await _context.SaveChangesAsync();
            }
        }

        // 🔹 الدالة الجديدة
        public async Task<Curriculum?> GetByGradeLevelAsync(string gradeLevel)
        {
            return await _context.Curriculums
                .Include(c => c.Courses)
                .FirstOrDefaultAsync(c => c.GradeLevel.ToString() == gradeLevel);
        }
    }

}
