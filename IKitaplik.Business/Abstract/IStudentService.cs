using Core.Utilities.Results;
using IKitaplik.Entities.Concrete;
using IKitaplik.Entities.DTOs;
using IKitaplik.Entities.DTOs.StudentDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKitaplik.Business.Abstract
{
    public interface IStudentService
    {
        Task<IResult> AddAsync(StudentAddDto studentAddDto);
        Task<IResult> UpdateAsync(StudentUpdateDto studentUpdateDto, bool isDonationOrDeposit = false);
        Task<IResult> DeleteAsync(int id);

        Task<IDataResult<PagedResult<StudentGetDto>>> GetAllAsync(int page,int pageSize);
        Task<IDataResult<PagedResult<StudentGetDto>>> GetAllActiveAsync(int page, int pageSize);
        Task<IDataResult<List<StudentGetDto>>> GetAllByNameAsync(string name);
        Task<IDataResult<Student>> GetByIdAsync(int id);
        Task<IDataResult<List<StudentAutocompleteDto>>> SearchForAutocompleteAsync(string query);
        Task<IDataResult<StudentGetDto>> GetDtoByIdAsync(int id);
    }
}
