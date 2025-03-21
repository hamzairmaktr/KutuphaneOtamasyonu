using Core.Utilities.Results;
using IKitaplik.Entities.Concrete;
using IKitaplik.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKitaplik.Business.Abstract
{
    public interface IStudentService
    {
        IResult Add(StudentAddDto studentAddDto);
        IResult Update(StudentUpdateDto studentUpdateDto);
        IResult Delete(int id);

        IDataResult<List<StudentGetDto>> GetAll();
        IDataResult<List<StudentGetDto>> GetAllByName(string name);
        IDataResult<Student> GetById(int id);
    }
}
