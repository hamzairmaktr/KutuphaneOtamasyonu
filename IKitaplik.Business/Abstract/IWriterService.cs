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
        IResult Add(WriterAddDto writerAddDto);
        IResult Delete(int id);
        IResult Update(WriterUpdateDto writerUpdateDto);

        IDataResult<List<WriterGetDto>> GetAllFilteredNameContains(string name);
        IDataResult<List<WriterGetDto>> GetAll();
        IDataResult<Writer> GetById(int id);
    }
}
