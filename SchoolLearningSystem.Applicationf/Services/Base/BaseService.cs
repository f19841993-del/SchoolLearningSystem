 using AutoMapper;
    using global::SchoolLearningSystem.Applicationf.Interfaces.Base;
    using global::SchoolLearningSystem.Domain.Interfaces.Base;
using SchoolLearningSystem.Application.Common.Models;
using SchoolLearningSystem.Application.Common.Parameters;
using SchoolLearningSystem.Applicationf.DTOs.CourseDto;
using SchoolLearningSystem.Applicationf.Exceptions;
using System.Linq.Expressions;


namespace SchoolLearningSystem.Applicationf.Services.Base
    {
        public abstract class BaseService<TEntity, TReadDto, TCreateDto, TUpdateDto>
            : IBaseService<TReadDto, TCreateDto, TUpdateDto>
            where TEntity : class
        {
            // نستخدم protected لكي تستطيع الكلاسات الوريثة (مثل CourseService) الوصول لها
            protected readonly IGenericRepository<TEntity> _repository;
            protected readonly IMapper _mapper;

            public BaseService(IGenericRepository<TEntity> repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public virtual async Task<IEnumerable<TReadDto>> GetAllAsync()
            {
                var entities = await _repository.GetAllAsync();
                return _mapper.Map<IEnumerable<TReadDto>>(entities);
            }

            public virtual async Task<TReadDto?> GetByIdAsync(int id)
            {
                var entity = await _repository.GetByIdAsync(id);
                return _mapper.Map<TReadDto>(entity);
            }

            public virtual async Task<TReadDto> CreateAsync(TCreateDto dto)
            {
                var entity = _mapper.Map<TEntity>(dto);
                await _repository.AddAsync(entity);
            // الآن، التغييرات تُحفظ وتتولد الـ IDs في قاعدة البيانات!
            await _repository.SaveChangesAsync();
            return _mapper.Map<TReadDto>(entity);
            }

            public virtual async Task UpdateAsync(int id, TUpdateDto dto)
            {
            // 1. جلب الكيان (هنا الـ entity يدخل في حالة Tracking)
            var entity = await _repository.GetByIdAsync(id);
                if (entity == null)throw new NotFoundException($"Entity with ID {id} not found");

            _mapper.Map(dto, entity);
                await _repository.UpdateAsync(entity);
            // 4. لا حاجة لاستدعاء _repository.UpdateAsync(entity) إذا كان الـ EF Core يعمل بوضع الـ Tracking
            // مجرد تعديل قيم الـ entity يكفي، لكن استدعاء UpdateAsync كـ "تأكيد" لا يضر.

            // 5. "المفتاح السحري" لتثبيت التغييرات
            await _repository.SaveChangesAsync();
        }

            public virtual async Task DeleteAsync(int id)
            {
                await _repository.DeleteAsync(id);
            await _repository.SaveChangesAsync();
        }

        // 🌟 لاحظ الإضافة: جعلنا الفلتر متغيراً اختيارياً نعطيه قيمة افتراضية null
        // 🌟 1. الدالة العامة (Public) التي يراها الـ Controller وتطبق الواجهة
        public virtual async Task<PagedList<TReadDto>> GetPagedAsync(QueryParameters parameters)
        {
            // تنادي الدالة المحمية (في الأسفل) وتمرر لها null لأنه لا يوجد فلتر خاص
            return await GetPagedWithFilterAsync(parameters, null);
        }

        // 🌟 2. الدالة المحمية (Protected) التي يراها الأبناء فقط (مثل CourseService)
        // لاحظ هنا أننا نستطيع استخدام TEntity براحة لأننا داخل الكلاس الذي يعرفه!
        protected async Task<PagedList<TReadDto>> GetPagedWithFilterAsync(
            QueryParameters parameters,
            Expression<Func<TEntity, bool>> filter)
        {
            var result = await _repository.GetPagedAsync(filter, parameters.PageNumber, parameters.PageSize);
            var itemsDto = _mapper.Map<IEnumerable<TReadDto>>(result.Items);
            return new PagedList<TReadDto>(itemsDto, result.TotalCount, parameters.PageNumber, parameters.PageSize);
        }





    }
    }

