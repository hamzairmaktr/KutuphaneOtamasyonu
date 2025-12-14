using Core.Utilities.Results;
using IKitaplik.Entities.Concrete;
using IKitaplik.Entities.DTOs.WriterDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKitaplik.Business.Abstract
{
    public interface IWriterService
    {
        Task<IResult> AddAsync(WriterAddDto writerAddDto);
        Task<IResult> DeleteAsync(int id);
        Task<IResult> UpdateAsync(WriterUpdateDto writerUpdateDto);

        Task<IDataResult<List<WriterGetDto>>> GetAllFilteredNameContainsAsync(string name);
        Task<IDataResult<PagedResult<WriterGetDto>>> GetAllAsync(int page, int pageSize);
        Task<IDataResult<Writer>> GetByIdAsync(int id);
    }
}
