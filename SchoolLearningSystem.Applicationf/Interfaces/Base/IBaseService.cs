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
    }
}
