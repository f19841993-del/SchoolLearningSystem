using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLearningSystem.Domain.Interfaces.Base
{
    // في مجلد SchoolLearningSystem.Domain.Interfaces
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        // 🔹 التعديل هنا: إضافة دالة الترقيم (Pagination)
        // سنستخدم الـ Tuple لجلب البيانات والعدد الكلي معاً بشكل نظيف
        // دالة ترقيم تقبل شرطاً (مثل: جلب طلاب كورس معين)
        Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(
            Expression<Func<T, bool>> filter,
            int pageNumber,
            int pageSize);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
        Task SaveChangesAsync();
    }
}
