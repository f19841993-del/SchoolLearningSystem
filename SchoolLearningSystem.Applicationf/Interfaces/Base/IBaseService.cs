using SchoolLearningSystem.Application.Common.Models;
using SchoolLearningSystem.Application.Common.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLearningSystem.Applicationf.Interfaces.Base
{
    public interface IBaseService<TReadDto, TCreateDto, TUpdateDto>
    {
        Task<IEnumerable<TReadDto>> GetAllAsync();
        Task<TReadDto?> GetByIdAsync(int id);
        Task<TReadDto> CreateAsync(TCreateDto dto);
        Task UpdateAsync(int id, TUpdateDto dto);
        Task DeleteAsync(int id);
        //Task<PagedList<TReadDto>> GetPagedAsync(int pageNumber, int pageSize);
        Task<PagedList<TReadDto>> GetPagedAsync(QueryParameters parameters); // 👈 تستقبل الكلاس
    }
}
