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
        IResult Add(Student student);
        IResult Update(Student student);
        IResult Delete(int id);

        IDataResult<List<Student>> GetAll();
        IDataResult<List<Student>> GetAllByName(string name);
        IDataResult<Student> GetById(int id);
    }
}
