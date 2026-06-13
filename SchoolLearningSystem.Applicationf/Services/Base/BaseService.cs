 using AutoMapper;
    using global::SchoolLearningSystem.Applicationf.Interfaces.Base;
    using global::SchoolLearningSystem.Domain.Interfaces.Base;
   

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
                return _mapper.Map<TReadDto>(entity);
            }

            public virtual async Task UpdateAsync(int id, TUpdateDto dto)
            {
                var entity = await _repository.GetByIdAsync(id);
                if (entity == null) throw new Exception("Entity not found");

                _mapper.Map(dto, entity);
                await _repository.UpdateAsync(entity);
            }

            public virtual async Task DeleteAsync(int id)
            {
                await _repository.DeleteAsync(id);
            }
        }
    }

