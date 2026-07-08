using Microsoft.EntityFrameworkCore;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Enums;
using SchoolLearningSystem.Domain.Interfaces;
using SchoolLearningSystem.Infrastructure.Data;
using SchoolLearningSystem.Infrastructure.Repositories.Base;

namespace SchoolLearningSystem.Infrastructure.Repositories
{
    public class ExerciseRepository : GenericRepository<Exercise>, IExerciseRepository
    {
        public ExerciseRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Exercise>> GetByLessonIdAsync(int lessonId)
        {
            return await _context.Exercises
                .AsNoTracking()
                .Where(e => e.LessonId == lessonId && !e.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Exercise>> GetByDifficultyAsync(DifficultyLevel difficulty)
        {
            return await _context.Exercises
                .AsNoTracking()
                .Where(e => e.Difficulty == difficulty && !e.IsDeleted)
                .ToListAsync();
        }
    }
}