using Microsoft.EntityFrameworkCore;
using SchoolLearningSystem.Domain.Interfaces.Base;
using SchoolLearningSystem.Infrastructure.Data;
using System; // 👈 ضرورية جداً للـ Func
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SchoolLearningSystem.Infrastructure.Repositories.Base
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        public GenericRepository(AppDbContext context) => _context = context;

        // دالة GetById تبقى متعقبة (Tracked) لأننا غالباً نجلب العنصر لنعدله أو نحذفه
        public async Task<T?> GetByIdAsync(int id) => await _context.Set<T>().FindAsync(id);

        // 🔹 الأداء الأقصى: أضفنا AsNoTracking للقراءة فقط
        public async Task<IEnumerable<T>> GetAllAsync()
            => await _context.Set<T>().AsNoTracking().ToListAsync();

        // 🔹 تنفيذ دالة الترقيم (Pagination)
        public async Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(
             Expression<Func<T, bool>> filter, int pageNumber, int pageSize)
        {
            // 1. تطبيق الفلتر مع إيقاف التتبع لتسريع الأداء (AsNoTracking)
            var query = filter != null
                ? _context.Set<T>().AsNoTracking().Where(filter)
                : _context.Set<T>().AsNoTracking();

            // 2. حساب العدد بعد الفلترة
            var totalCount = await query.CountAsync();

            // 3. جلب البيانات المفلترة والمرقمة من قاعدة البيانات مباشرة!
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task AddAsync(T entity) => await _context.Set<T>().AddAsync(entity);

        // 🔹 التعديل الهندسي: إلغاء الـ async لتجنب التحذير (Compiler Warning)
        public Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            return Task.CompletedTask; // نرجع Task مكتمل وهمي لتلبية شروط الواجهة
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null) _context.Set<T>().Remove(entity);
        }

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}