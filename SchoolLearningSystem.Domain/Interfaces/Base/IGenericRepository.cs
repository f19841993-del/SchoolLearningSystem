using System.Linq.Expressions;

namespace SchoolLearningSystem.Domain.Interfaces.Base
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();

        // 🔹 دالة الترقيم (Pagination) - الفلتر اختياري (null = بدون فلترة إضافية)
        // 💡 Nullable ضرورية لتطابق GetPagedAsync(parameters) العامة بـ BaseService،
        // التي تمرر null عند عدم وجود فلتر خاص - بدونها Compile Error فوري.
        Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(
            Expression<Func<T, bool>>? filter,
            int pageNumber,
            int pageSize);

        Task AddAsync(T entity);
        Task UpdateAsync(T entity);

        // 🔹 حذف منطقي (Soft Delete) - الافتراضي لحماية البيانات
        Task DeleteAsync(int id);

        // 🆕 استرجاع سجل تم حذفه منطقياً (Soft Delete Undo)
        Task RestoreAsync(int id);

        // 🆕 حذف فعلي نهائي من قاعدة البيانات (استخدام نادر جداً - Admin فقط)
        Task HardDeleteAsync(int id);

        Task SaveChangesAsync();
    }
}