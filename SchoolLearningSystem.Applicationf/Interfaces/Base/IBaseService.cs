using SchoolLearningSystem.Applicationf.Common.Models;
using SchoolLearningSystem.Applicationf.Common.Parameters;

namespace SchoolLearningSystem.Applicationf.Interfaces.Base
{
    public interface IBaseService<TReadDto, TCreateDto, TUpdateDto>
    {
        Task<IEnumerable<TReadDto>> GetAllAsync();
        Task<TReadDto?> GetByIdAsync(int id);
        Task<TReadDto> CreateAsync(TCreateDto dto);
        Task UpdateAsync(int id, TUpdateDto dto);
        Task DeleteAsync(int id);

        // الترقيم مع دعم الفلترة/الترتيب عبر QueryParameters
        Task<PagedList<TReadDto>> GetPagedAsync(QueryParameters parameters);
    }
}