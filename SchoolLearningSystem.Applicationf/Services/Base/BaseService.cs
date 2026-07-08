using AutoMapper;
using SchoolLearningSystem.Applicationf.Common.Models;
using SchoolLearningSystem.Applicationf.Common.Parameters;
using SchoolLearningSystem.Applicationf.Exceptions;
using SchoolLearningSystem.Applicationf.Interfaces.Base;
using SchoolLearningSystem.Domain.Interfaces.Base;
using System.Linq.Expressions;

namespace SchoolLearningSystem.Applicationf.Services.Base
{
    // ==================================================================================
    // 📌 دور هذا الـ Service:
    // القاعدة العامة (Generic Base) لكل خدمات CRUD بالمشروع. توفّر التنفيذ الافتراضي
    // للعمليات الأساسية (قراءة، إنشاء، تعديل، حذف منطقي، ترقيم) لأي كيان، بحيث كل
    // Service مشتق (مثل CourseService) يحصل عليها مجاناً دون تكرار الكود.
    // ==================================================================================
    public abstract class BaseService<TEntity, TReadDto, TCreateDto, TUpdateDto>
        : IBaseService<TReadDto, TCreateDto, TUpdateDto>
        where TEntity : class
    {
        // protected تسمح للكلاسات الوريثة (مثل CourseService) بالوصول المباشر لها
        protected readonly IGenericRepository<TEntity> _repository;
        protected readonly IMapper _mapper;

        public BaseService(IGenericRepository<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // ============================================================================
        // 🎯 Use Case: "أي شاشة تعرض قائمة كاملة من عناصر كيان معيّن" (مثل: كل الكورسات،
        //              كل المعلمين...) - بدون ترقيم، تُستخدم للقوائم الصغيرة نسبياً.
        // ============================================================================
        public virtual async Task<IEnumerable<TReadDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<TReadDto>>(entities);
        }

        // ============================================================================
        // 🎯 Use Case: "المستخدم يفتح صفحة تفاصيل عنصر معيّن بمعرفة رقمه (Id)"
        // 💡 Nullable لأن الـ Controller قد يحتاج يعرض 404 بنفسه بدل استثناء موحّد.
        // ============================================================================
        public virtual async Task<TReadDto?> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return _mapper.Map<TReadDto>(entity);
        }

        // ============================================================================
        // 🎯 Use Case: "المستخدم يعبّئ نموذجاً (Form) لإنشاء عنصر جديد ويضغط حفظ"
        // ============================================================================
        public virtual async Task<TReadDto> CreateAsync(TCreateDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            await _repository.AddAsync(entity);

            // 💡 المكان الوحيد لاستدعاء SaveChangesAsync (Unit of Work)
            await _repository.SaveChangesAsync();

            return _mapper.Map<TReadDto>(entity);
        }

        // ============================================================================
        // 🎯 Use Case: "المستخدم يعدّل بيانات عنصر موجود مسبقاً ويضغط حفظ التعديلات"
        // ============================================================================
        public virtual async Task UpdateAsync(int id, TUpdateDto dto)
        {
            // 1. جلب الكيان (يدخل في حالة Tracking تلقائياً عبر EF Core)
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                throw new NotFoundException($"Entity with ID {id} not found");

            // 2. تطبيق التعديلات من الـ DTO على الكيان المُتتبَّع
            _mapper.Map(dto, entity);
            await _repository.UpdateAsync(entity);

            // 3. المكان الوحيد لتثبيت التغييرات
            await _repository.SaveChangesAsync();
        }

        // ============================================================================
        // 🎯 Use Case: "الأدمن أو المستخدم يحذف عنصراً (حذف منطقي دائماً، لا نهائي)"
        // 💡 تحقق صريح من الوجود قبل الحذف لضمان اتساق السلوك مع UpdateAsync (كلاهما
        //    يرمي NotFoundException بنفس الطريقة، لا سلوك صامت مختلف بينهما).
        // ============================================================================
        public virtual async Task DeleteAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                throw new NotFoundException($"Entity with ID {id} not found");

            await _repository.DeleteAsync(id);
            await _repository.SaveChangesAsync();
        }

        // ============================================================================
        // 🎯 Use Case: "شاشة قائمة كبيرة (مئات/آلاف العناصر) تحتاج ترقيماً بالصفحات
        //              بدون فلتر خاص" - هذي الدالة العامة التي يراها الـ Controller.
        // ============================================================================
        public virtual async Task<PagedList<TReadDto>> GetPagedAsync(QueryParameters parameters)
        {
            // تنادي الدالة المحمية وتمرر لها null لأنه لا يوجد فلتر خاص
            return await GetPagedWithFilterAsync(parameters, null);
        }

        // ============================================================================
        // 🎯 Use Case: "خدمة مشتقة (مثل CourseStudentService) تحتاج ترقيماً بشرط
        //              خاص" (مثلاً: طلاب كورس معيّن فقط) - دالة داخلية للأبناء فقط.
        // ============================================================================
        protected async Task<PagedList<TReadDto>> GetPagedWithFilterAsync(
            QueryParameters parameters,
            Expression<Func<TEntity, bool>>? filter)
        {
            var result = await _repository.GetPagedAsync(filter, parameters.PageNumber, parameters.PageSize);
            var itemsDto = _mapper.Map<IEnumerable<TReadDto>>(result.Items);
            return new PagedList<TReadDto>(itemsDto, result.TotalCount, parameters.PageNumber, parameters.PageSize);
        }
    }
}