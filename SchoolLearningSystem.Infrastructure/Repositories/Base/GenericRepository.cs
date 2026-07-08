using Microsoft.EntityFrameworkCore;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Interfaces.Base;
using SchoolLearningSystem.Infrastructure.Data;
using System.Linq.Expressions;

namespace SchoolLearningSystem.Infrastructure.Repositories.Base
{
    // ==================================================================================
    // 📌 قرارات معمارية مطبّقة بهذا الملف (نتيجة كل النقاشات السابقة):
    // 1. القيد where T : BaseEntity ضروري للوصول إلى Id/IsDeleted بأمان وقت الترجمة.
    // 2. لا يوجد فلتر !IsDeleted يدوي هنا لأن AppDbContext يطبّق Global Query Filter
    //    تلقائياً على كل الاستعلامات العادية لأي Entity يرث BaseEntity.
    // 3. SaveChangesAsync لا تُستدعى من أي دالة هنا - هي مسؤولية الـ Service حصراً
    //    (Unit of Work)، لتفادي رحلتين لقاعدة البيانات بنفس العملية المنطقية.
    // 4. RestoreAsync/HardDeleteAsync تستخدم IgnoreQueryFilters() لأنها الوحيدة التي
    //    تحتاج الوصول لسجلات IsDeleted=true (المستبعدة تلقائياً من كل استعلام آخر).
    // ==================================================================================
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            // الفلتر العام بـ AppDbContext يستبعد IsDeleted=true تلقائياً هنا
            return await _dbSet.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(
            Expression<Func<T, bool>>? filter, // 💡 Nullable - يمنع ArgumentNullException
            int pageNumber,
            int pageSize)
        {
            var query = _dbSet.AsNoTracking();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(e => e.Id) // ترتيب ثابت ضروري مع Skip/Take
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        // 💡 بدون SaveChangesAsync هنا - تُستدعى من الـ Service بعد هذي الدالة
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public Task UpdateAsync(T entity)
        {
            entity.LastModifiedAt = DateTime.UtcNow;
            _dbSet.Update(entity);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _dbSet.FirstOrDefaultAsync(e => e.Id == id);
            if (entity != null && !entity.IsDeleted)
            {
                entity.IsDeleted = true;
                entity.LastModifiedAt = DateTime.UtcNow;
            }
        }

        // ⚠️ IgnoreQueryFilters() ضرورية: السجل IsDeleted=true، والفلتر العام
        // يستبعده تلقائياً من أي استعلام عادي. بدون هذا التجاوز الصريح، الاسترجاع
        // يفشل بصمت دائماً (يرجع null حتى لو السجل موجود فعلياً بقاعدة البيانات).
        public async Task RestoreAsync(int id)
        {
            var entity = await _dbSet.IgnoreQueryFilters().FirstOrDefaultAsync(e => e.Id == id);
            if (entity != null && entity.IsDeleted)
            {
                entity.IsDeleted = false;
                entity.LastModifiedAt = DateTime.UtcNow;
            }
        }

        // ⚠️ نفس السبب: نحتاج الوصول للسجل بغض النظر عن حالة IsDeleted لحذفه نهائياً
        public async Task HardDeleteAsync(int id)
        {
            var entity = await _dbSet.IgnoreQueryFilters().FirstOrDefaultAsync(e => e.Id == id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}